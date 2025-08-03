using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Linq;
using RemoteMergeUtility.Models;

namespace RemoteMergeUtility.ViewModels
{
	public class ProjectManagerViewModel : ViewModelBase
	{
		private ProjectInfo? _SelectedProject;
		private readonly ObservableCollection<ProjectInfo> _PROJECTS;

		public ProjectManagerViewModel()
		{
			_PROJECTS = new ObservableCollection<ProjectInfo>();
			AddProjectCommand = new RelayCommand(ExecuteAddProject);
			RemoveProjectCommand = new RelayCommand(ExecuteRemoveProject, CanExecuteRemoveProject);
			
			// 初期状態でDefaultプロジェクトを確保
			EnsureDefaultProject();
		}

		public ObservableCollection<ProjectInfo> Projects => _PROJECTS;

		public ProjectInfo? SelectedProject
		{
			get => _SelectedProject;
			set => SetProperty(ref _SelectedProject, value);
		}

		public ICommand AddProjectCommand { get; }
		public ICommand RemoveProjectCommand { get; }

		private void ExecuteAddProject()
		{
			var newProject = new ProjectInfo()
			{
				Key = "新しいプロジェクト",
				Path = ""
			};
			Projects.Add(newProject);
			SelectedProject = newProject;
		}

		private void ExecuteRemoveProject()
		{
			if (SelectedProject != null)
			{
				Projects.Remove(SelectedProject);
				SelectedProject = Projects.Count > 0 ? Projects[0] : null;
			}
		}

		private bool CanExecuteRemoveProject()
		{
			return SelectedProject != null && !SelectedProject.IsDefault;
		}

		private void EnsureDefaultProject()
		{
			// Defaultプロジェクトが存在しない場合は作成
			var defaultProject = _PROJECTS.FirstOrDefault(p => p.IsDefault);
			if (defaultProject == null)
			{
				var newDefaultProject = new ProjectInfo(true)
				{
					Key = "Default",
					Path = ""
				};
				_PROJECTS.Insert(0, newDefaultProject);
			}
			else
			{
				// 既存のDefaultプロジェクトを先頭に移動
				var currentIndex = _PROJECTS.IndexOf(defaultProject);
				if (currentIndex > 0)
				{
					_PROJECTS.Move(currentIndex, 0);
				}
			}
		}

		public void LoadProjects(IEnumerable<ProjectInfo> projects)
		{
			_PROJECTS.Clear();
			foreach (var project in projects)
			{
				_PROJECTS.Add(project);
			}
			EnsureDefaultProject();
		}
	}
}