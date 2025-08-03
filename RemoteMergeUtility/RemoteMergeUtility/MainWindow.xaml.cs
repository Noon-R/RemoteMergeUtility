using System.Windows;
using RemoteMergeUtility.ViewModels;
using RemoteMergeUtility.Services;

namespace RemoteMergeUtility
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			
			var dataService = new JsonProjectDataService();
			var viewModel = new ProjectManagerViewModel();
			
			// データコンテキストの設定
			DataContext = viewModel;
			
			// ウィンドウを最初は非表示にする
			WindowState = WindowState.Minimized;
			ShowInTaskbar = false;
			Visibility = Visibility.Hidden;
			
			// ウィンドウクローズ時はタスクトレイに隠す
			Closing += async (sender, e) =>
			{
				e.Cancel = true; // 実際のクローズをキャンセル
				Hide();
				await dataService.SaveProjectsAsync(viewModel.Projects);
			};
			
			// アプリケーション開始時のデータ読み込み
			Loaded += async (sender, e) =>
			{
				var projects = await dataService.LoadProjectsAsync();
				viewModel.LoadProjects(projects);
			};
		}

		public void ShowEditWindow()
		{
			Show();
			WindowState = WindowState.Normal;
			Activate();
			Topmost = true;
			Topmost = false;
		}
	}
}