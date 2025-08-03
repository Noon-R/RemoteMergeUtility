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
			
			// �f�[�^�R���e�L�X�g��ݒ�
			DataContext = viewModel;
			
			// �A�v���P�[�V�����I�����̃f�[�^�ۑ�
			Closing += async (sender, e) =>
			{
				await dataService.SaveProjectsAsync(viewModel.Projects);
			};
			
			// �A�v���P�[�V�����J�n���̃f�[�^�ǂݍ���
			Loaded += async (sender, e) =>
			{
				var projects = await dataService.LoadProjectsAsync();
				viewModel.LoadProjects(projects);
			};
		}
	}
}