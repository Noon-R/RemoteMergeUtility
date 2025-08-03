using System.Collections.ObjectModel;
using System.Windows.Input;
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
            var newProject = new ProjectInfo
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
            return SelectedProject != null;
        }
    }
}