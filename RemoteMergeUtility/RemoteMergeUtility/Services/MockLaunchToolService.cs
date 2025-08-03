using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RemoteMergeUtility.Services
{
	public class MockLaunchToolService : ILaunchToolService
	{
		private readonly Dictionary<string, string> _MOCK_PROJECT_LIST = new Dictionary<string, string>
		{
			{ "Default", @"C:\Projects\Default" },
			{ "MyProject", @"C:\Projects\MyProject" },
			{ "TestProj", @"C:\Projects\TestProject" },
			{ "DevProject", @"C:\Dev\SampleApp" }
		};

		private readonly HashSet<string> _RUNNING_PROJECTS = new HashSet<string>
		{
			"MyProject"  // デバッグ用：MyProjectは起動済みとして扱う
		};

		public Task<Dictionary<string, string>> GetProjectListAsync()
		{
			// モック：固定のプロジェクトリストを返す
			return Task.FromResult(new Dictionary<string, string>(_MOCK_PROJECT_LIST));
		}

		public Task<bool> IsProjectRunningAsync(string projectName)
		{
			// モック：事前定義された起動状態を返す
			var isRunning = _RUNNING_PROJECTS.Contains(projectName);
			
			// デバッグ情報をログに出力
			LogService.Debug($"[MOCK] IsProjectRunning('{projectName}') = {isRunning}");
			
			return Task.FromResult(isRunning);
		}

		public Task<bool> LaunchProjectAsync(string projectName)
		{
			// モック：常に成功とし、起動済みリストに追加
			_RUNNING_PROJECTS.Add(projectName);
			
			// デバッグ情報をログに出力
			LogService.Debug($"[MOCK] LaunchProject('{projectName}') = Success");
			
			// 遅延をシミュレート
			return Task.Delay(500).ContinueWith(_ => true);
		}

		public Task<bool> SendHttpRequestAsync(string projectName, int revision, string? args)
		{
			// モック：常に成功として扱う
			
			// デバッグ情報をログに出力
			LogService.Debug($"[MOCK] SendHttpRequest('{projectName}', revision={revision}, args='{args}') = Success");
			
			// 遅延をシミュレート
			return Task.Delay(300).ContinueWith(_ => true);
		}

		public void Dispose()
		{
			// モック：何もしない
			LogService.Debug("[MOCK] MockLaunchToolService disposed");
		}
	}
}