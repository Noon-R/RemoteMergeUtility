using System.Data;
using System.Windows;
using System.IO;
using System.IO.Pipes;
using System.Text;
using RemoteMergeUtility.Services;
using RemoteMergeUtility.Models;
using Application = System.Windows.Application;

namespace RemoteMergeUtility
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		private readonly UrlSchemeService _URL_SCHEME_SERVICE = new UrlSchemeService();
		private readonly JsonProjectDataService _DATA_SERVICE = new JsonProjectDataService();
		private readonly ILaunchToolService _LAUNCH_TOOL_SERVICE;
		private IEnumerable<ProjectInfo> _LOADED_PROJECTS = new List<ProjectInfo>();
		
		private static Mutex _INSTANCE_MUTEX;
		private const string MUTEX_NAME = "RemoteMergeUtility_SingleInstance";
		private const string PIPE_NAME = "RemoteMergeUtility_UrlScheme";
		private NamedPipeServerStream _PIPE_SERVER;
		private NotifyIcon _NOTIFY_ICON;

		public App()
		{
			// ビルド構成に応じてサービスを切り替え
#if DEBUG
			_LAUNCH_TOOL_SERVICE = new MockLaunchToolService();
			System.Diagnostics.Debug.WriteLine("[APP] Using MockLaunchToolService (DEBUG build)");
#else
			_LAUNCH_TOOL_SERVICE = new LaunchToolService();
			System.Diagnostics.Debug.WriteLine("[APP] Using LaunchToolService (RELEASE build)");
#endif
		}

		protected override async void OnStartup(StartupEventArgs e)
		{
			// 単一インスタンス制御
			bool isNewInstance;
			_INSTANCE_MUTEX = new Mutex(true, MUTEX_NAME, out isNewInstance);

			if (!isNewInstance)
			{
				// 既存インスタンスが存在する場合
				if (e.Args.Length > 0)
				{
					// URLスキーマの場合は既存インスタンスに処理を委譲
					await SendToExistingInstance(e.Args);
				}
				
				// 新しいインスタンスを終了
				Shutdown();
				return;
			}

			base.OnStartup(e);

			// ProjectInfoを最初に読み込み
			await LoadProjectsAsync();

			// コマンドライン引数をチェック
			if (e.Args.Length > 0)
			{
				ProcessCommandLineArgs(e.Args);
			}

			// パイプサーバー開始
			StartPipeServer();

			// タスクトレイ初期化
			InitializeSystemTray();
		}

		private async Task LoadProjectsAsync()
		{
			try
			{
				_LOADED_PROJECTS = await _DATA_SERVICE.LoadProjectsAsync();
			}
			catch
			{
				// エラーの場合は空のリストを使用
				_LOADED_PROJECTS = new List<ProjectInfo>();
			}
		}

		private void ProcessCommandLineArgs(string[] args)
		{
			// 最初の引数がURLスキーマかチェック
			var firstArg = args[0];
			
			if (_URL_SCHEME_SERVICE.IsValidScheme(firstArg))
			{
				var urlRequest = _URL_SCHEME_SERVICE.ParseUrl(firstArg);
				if (urlRequest != null)
				{
					ProcessUrlSchemeRequest(urlRequest);
					return;
				}
			}

			// 通常のコマンドライン引数処理
			ProcessRegularCommandLine(args);
		}

		private async void ProcessUrlSchemeRequest(UrlSchemeRequest request)
		{
			// 読み込み済みのProjectInfoからtargetを検索
			var targetProject = _LOADED_PROJECTS.FirstOrDefault(p => p.Key == request.Target);
			
			if (targetProject == null)
			{
				System.Windows.MessageBox.Show($"エラー: プロジェクト '{request.Target}' が見つかりません。\n\n利用可能なプロジェクト:\n{string.Join("\n", _LOADED_PROJECTS.Select(p => $"- {p.Key}"))}", 
					"URL Schema Handler - エラー", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
				return;
			}

			try
			{
				// 1. 外部ツールからプロジェクトリストを取得してプロジェクト名を特定
				var projectList = await _LAUNCH_TOOL_SERVICE.GetProjectListAsync();
				var projectName = GetProjectNameFromPath(projectList, targetProject.Path);
				
				if (string.IsNullOrEmpty(projectName))
				{
					System.Windows.MessageBox.Show($"エラー: プロジェクトパス '{targetProject.Path}' に対応するプロジェクト名が見つかりません。", 
						"URL Schema Handler - エラー", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
					return;
				}

				// 2. プロジェクトが起動済みかを確認
				var isRunning = await _LAUNCH_TOOL_SERVICE.IsProjectRunningAsync(projectName);

				if (!isRunning)
				{
					// 3a. 起動していない場合：プロジェクトに命令を出す
					var launchResult = await _LAUNCH_TOOL_SERVICE.LaunchProjectAsync(projectName);
					
					if (!launchResult)
					{
						System.Windows.MessageBox.Show($"エラー: プロジェクト '{projectName}' の起動に失敗しました。", 
							"URL Schema Handler - エラー", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
						return;
					}

					System.Windows.MessageBox.Show($"プロジェクト '{projectName}' を起動しました。\nRevision: {request.Revision}\nArgs: {request.Args}", 
						"URL Schema Handler - 起動完了", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
				}
				else
				{
					// 3b. 起動している場合：HTTPリクエストをPost
					var httpResult = await _LAUNCH_TOOL_SERVICE.SendHttpRequestAsync(projectName, request.Revision, request.Args);
					
					if (!httpResult)
					{
						System.Windows.MessageBox.Show($"エラー: プロジェクト '{projectName}' へのHTTPリクエスト送信に失敗しました。", 
							"URL Schema Handler - エラー", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
						return;
					}

					System.Windows.MessageBox.Show($"プロジェクト '{projectName}' にHTTPリクエストを送信しました。\nRevision: {request.Revision}\nArgs: {request.Args}", 
						"URL Schema Handler - 送信完了", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
				}
			}
			catch (Exception ex)
			{
				System.Windows.MessageBox.Show($"処理中にエラーが発生しました:\n{ex.Message}", 
					"URL Schema Handler - エラー", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
			}
		}

		private string GetProjectNameFromPath(Dictionary<string, string> projectList, string targetPath)
		{
			// プロジェクトリストから対象パスに一致するプロジェクト名を検索
			foreach (var project in projectList)
			{
				if (string.Equals(project.Value, targetPath, StringComparison.OrdinalIgnoreCase))
				{
					return project.Key;
				}
			}

			// 完全一致しない場合、パスの一部が含まれているかチェック
			foreach (var project in projectList)
			{
				if (targetPath.Contains(project.Value, StringComparison.OrdinalIgnoreCase) ||
					project.Value.Contains(targetPath, StringComparison.OrdinalIgnoreCase))
				{
					return project.Key;
				}
			}

			return string.Empty;
		}

		private void ProcessRegularCommandLine(string[] args)
		{
			// 通常のコマンドライン引数処理
			// 必要に応じて実装
		}

		private async Task SendToExistingInstance(string[] args)
		{
			try
			{
				using var pipeClient = new NamedPipeClientStream(".", PIPE_NAME, PipeDirection.Out);
				await pipeClient.ConnectAsync(5000); // 5秒タイムアウト

				var message = string.Join("|", args);
				var data = Encoding.UTF8.GetBytes(message);
				await pipeClient.WriteAsync(data, 0, data.Length);
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"[APP] Failed to send to existing instance: {ex.Message}");
			}
		}

		private void StartPipeServer()
		{
			Task.Run(async () =>
			{
				while (true)
				{
					try
					{
						_PIPE_SERVER = new NamedPipeServerStream(PIPE_NAME, PipeDirection.In);
						await _PIPE_SERVER.WaitForConnectionAsync();

						using var reader = new StreamReader(_PIPE_SERVER, Encoding.UTF8);
						var message = await reader.ReadToEndAsync();
						
						if (!string.IsNullOrEmpty(message))
						{
							var args = message.Split('|');
							
							// UIスレッドで実行
							Dispatcher.Invoke(() =>
							{
								ProcessCommandLineArgs(args);
								
								// URLスキーマ処理時はウィンドウを表示しない（タスクトレイのまま）
								// 必要に応じてノティフィケーションを表示
							});
						}

						_PIPE_SERVER.Disconnect();
					}
					catch (Exception ex)
					{
						System.Diagnostics.Debug.WriteLine($"[APP] Pipe server error: {ex.Message}");
					}
					finally
					{
						_PIPE_SERVER?.Dispose();
					}
				}
			});
		}

		private void InitializeSystemTray()
		{
			_NOTIFY_ICON = new NotifyIcon
			{
				Icon = System.Drawing.SystemIcons.Application,
				Visible = true,
				Text = "RemoteMergeUtility"
			};

			// 右クリックメニューの作成
			var contextMenu = new ContextMenuStrip();
			
			var editMenuItem = new ToolStripMenuItem("プロジェクト編集(&E)");
			editMenuItem.Click += (sender, e) =>
			{
				Dispatcher.Invoke(() =>
				{
					var mainWindow = MainWindow as MainWindow;
					mainWindow?.ShowEditWindow();
				});
			};

			var exitMenuItem = new ToolStripMenuItem("終了(&X)");
			exitMenuItem.Click += (sender, e) =>
			{
				Dispatcher.Invoke(() =>
				{
					_NOTIFY_ICON.Visible = false;
					Shutdown();
				});
			};

			contextMenu.Items.Add(editMenuItem);
			contextMenu.Items.Add(new ToolStripSeparator());
			contextMenu.Items.Add(exitMenuItem);
			
			_NOTIFY_ICON.ContextMenuStrip = contextMenu;

			// ダブルクリックで編集画面を表示
			_NOTIFY_ICON.DoubleClick += (sender, e) =>
			{
				Dispatcher.Invoke(() =>
				{
					var mainWindow = MainWindow as MainWindow;
					mainWindow?.ShowEditWindow();
				});
			};
		}

		protected override void OnExit(ExitEventArgs e)
		{
			// リソースの解放
			_LAUNCH_TOOL_SERVICE?.Dispose();
			_PIPE_SERVER?.Dispose();
			_NOTIFY_ICON?.Dispose();
			_INSTANCE_MUTEX?.ReleaseMutex();
			_INSTANCE_MUTEX?.Dispose();
			base.OnExit(e);
		}
	}
}
