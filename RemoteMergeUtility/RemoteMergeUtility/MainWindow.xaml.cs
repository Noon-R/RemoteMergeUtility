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
            
            // データコンテキストを設定
            DataContext = viewModel;
            
            // アプリケーション終了時のデータ保存
            Closing += async (sender, e) =>
            {
                await dataService.SaveProjectsAsync(viewModel.Projects);
            };
            
            // アプリケーション開始時のデータ読み込み
            Loaded += async (sender, e) =>
            {
                var projects = await dataService.LoadProjectsAsync();
                foreach (var project in projects)
                {
                    viewModel.Projects.Add(project);
                }
            };
        }
    }
}