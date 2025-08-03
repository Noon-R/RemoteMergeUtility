using System.Configuration;
using System.Data;
using System.Windows;
using System.Linq;
using RemoteMergeUtility.Services;

namespace RemoteMergeUtility
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		private readonly UrlSchemeService _URL_SCHEME_SERVICE = new UrlSchemeService();

		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

			// コマンドライン引数をチェック
			if (e.Args.Length > 0)
			{
				ProcessCommandLineArgs(e.Args);
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

		private void ProcessUrlSchemeRequest(Models.UrlSchemeRequest request)
		{
			// TODO: URLスキーマリクエストの処理を実装
			// この段階ではメインウィンドウを表示し、後でProjectManagerViewModelに
			// リクエストを渡す仕組みを実装する必要がある
			
			// デバッグ用メッセージ（実装時に削除）
			MessageBox.Show($"URLスキーマ受信:\nTarget: {request.Target}\nRevision: {request.Revision}\nArgs: {request.Args}", 
				"URL Schema Handler", MessageBoxButton.OK, MessageBoxImage.Information);
		}

		private void ProcessRegularCommandLine(string[] args)
		{
			// 通常のコマンドライン引数処理
			// 必要に応じて実装
		}
	}
}
