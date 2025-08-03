using System;
using System.Configuration;
using System.Data;
using System.Windows;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using RemoteMergeUtility.Services;
using RemoteMergeUtility.Models;

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
			base.OnStartup(e);

			// ProjectInfoを最初に読み込み
			await LoadProjectsAsync();

			// コマンドライン引数をチェック
			if (e.Args.Length > 0)
			{
				ProcessCommandLineArgs(e.Args);
			}
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
				MessageBox.Show($"エラー: プロジェクト '{request.Target}' が見つかりません。\n\n利用可能なプロジェクト:\n{string.Join("\n", _LOADED_PROJECTS.Select(p => $"- {p.Key}"))}", 
					"URL Schema Handler - エラー", MessageBoxButton.OK, MessageBoxImage.Warning);
				return;
			}

			try
			{
				// 1. 外部ツールからプロジェクトリストを取得してプロジェクト名を特定
				var projectList = await _LAUNCH_TOOL_SERVICE.GetProjectListAsync();
				var projectName = GetProjectNameFromPath(projectList, targetProject.Path);
				
				if (string.IsNullOrEmpty(projectName))
				{
					MessageBox.Show($"エラー: プロジェクトパス '{targetProject.Path}' に対応するプロジェクト名が見つかりません。", 
						"URL Schema Handler - エラー", MessageBoxButton.OK, MessageBoxImage.Warning);
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
						MessageBox.Show($"エラー: プロジェクト '{projectName}' の起動に失敗しました。", 
							"URL Schema Handler - エラー", MessageBoxButton.OK, MessageBoxImage.Error);
						return;
					}

					MessageBox.Show($"プロジェクト '{projectName}' を起動しました。\nRevision: {request.Revision}\nArgs: {request.Args}", 
						"URL Schema Handler - 起動完了", MessageBoxButton.OK, MessageBoxImage.Information);
				}
				else
				{
					// 3b. 起動している場合：HTTPリクエストをPost
					var httpResult = await _LAUNCH_TOOL_SERVICE.SendHttpRequestAsync(projectName, request.Revision, request.Args);
					
					if (!httpResult)
					{
						MessageBox.Show($"エラー: プロジェクト '{projectName}' へのHTTPリクエスト送信に失敗しました。", 
							"URL Schema Handler - エラー", MessageBoxButton.OK, MessageBoxImage.Error);
						return;
					}

					MessageBox.Show($"プロジェクト '{projectName}' にHTTPリクエストを送信しました。\nRevision: {request.Revision}\nArgs: {request.Args}", 
						"URL Schema Handler - 送信完了", MessageBoxButton.OK, MessageBoxImage.Information);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"処理中にエラーが発生しました:\n{ex.Message}", 
					"URL Schema Handler - エラー", MessageBoxButton.OK, MessageBoxImage.Error);
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

		protected override void OnExit(ExitEventArgs e)
		{
			// リソースの解放
			_LAUNCH_TOOL_SERVICE?.Dispose();
			base.OnExit(e);
		}
	}
}
