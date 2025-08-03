using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RemoteMergeUtility.Services
{
	public interface ILaunchToolService : IDisposable
	{
		Task<Dictionary<string, string>> GetProjectListAsync();
		Task<bool> IsProjectRunningAsync(string projectName);
		Task<bool> LaunchProjectAsync(string projectName);
		Task<bool> SendHttpRequestAsync(string projectName, int revision, string? args);
	}
}