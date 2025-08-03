using System.Collections.Generic;
using System.Threading.Tasks;
using RemoteMergeUtility.Models;

namespace RemoteMergeUtility.Services
{
    public interface IProjectDataService
    {
        Task<List<ProjectInfo>> LoadProjectsAsync();
        Task SaveProjectsAsync(IEnumerable<ProjectInfo> projects);
    }
}