using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RemoteMergeUtility.Services
{
	public class LaunchToolService : ILaunchToolService, IDisposable
	{
		private readonly HttpClient _HTTP_CLIENT = new HttpClient();

		public async Task<Dictionary<string, string>> GetProjectListAsync()
		{
			try
			{
				var result = await ExecuteCommandAsync("launch -project-list");
				var projectList = new Dictionary<string, string>();

				// 仮想的な出力解析（実際の実装では外部ツールの出力形式に合わせる）
				if (!string.IsNullOrEmpty(result))
				{
					var lines = result.Split('\n', StringSplitOptions.RemoveEmptyEntries);
					foreach (var line in lines)
					{
						var parts = line.Split(':', 2);
						if (parts.Length == 2)
						{
							projectList[parts[0].Trim()] = parts[1].Trim();
						}
					}
				}

				return projectList;
			}
			catch
			{
				return new Dictionary<string, string>();
			}
		}

		public async Task<bool> IsProjectRunningAsync(string projectName)
		{
			try
			{
				var result = await ExecuteCommandAsync($"launch -name {projectName}");
				
				// 仮想的な出力解析（"running" または "stopped" を想定）
				return result?.Trim().ToLower().Contains("running") == true;
			}
			catch
			{
				return false;
			}
		}

		public async Task<bool> LaunchProjectAsync(string projectName)
		{
			try
			{
				var result = await ExecuteCommandAsync($"launch -project {projectName}");
				
				// 仮想的な成功判定
				return !string.IsNullOrEmpty(result) && !result.Contains("error");
			}
			catch
			{
				return false;
			}
		}

		public async Task<bool> SendHttpRequestAsync(string projectName, int revision, string? args)
		{
			try
			{
				// プロジェクト名からHTTPエンドポイントを推定（仮想的な実装）
				var baseUrl = GetProjectHttpEndpoint(projectName);
				if (string.IsNullOrEmpty(baseUrl))
					return false;

				var requestData = new
				{
					revision = revision,
					args = args ?? string.Empty,
					timestamp = DateTime.UtcNow
				};

				var json = JsonSerializer.Serialize(requestData);
				var content = new StringContent(json, Encoding.UTF8, "application/json");

				var response = await _HTTP_CLIENT.PostAsync($"{baseUrl}/api/merge", content);
				return response.IsSuccessStatusCode;
			}
			catch
			{
				return false;
			}
		}

		private async Task<string> ExecuteCommandAsync(string command)
		{
			try
			{
				var processInfo = new ProcessStartInfo
				{
					FileName = "cmd.exe",
					Arguments = $"/c {command}",
					UseShellExecute = false,
					RedirectStandardOutput = true,
					RedirectStandardError = true,
					CreateNoWindow = true
				};

				using var process = Process.Start(processInfo);
				if (process != null)
				{
					var output = await process.StandardOutput.ReadToEndAsync();
					await process.WaitForExitAsync();
					return output;
				}
			}
			catch
			{
				// エラーハンドリング
			}

			return string.Empty;
		}

		private string GetProjectHttpEndpoint(string projectName)
		{
			// 仮想的なプロジェクト名からHTTPエンドポイントのマッピング
			// 実際の実装では設定ファイルやレジストリから取得
			return projectName switch
			{
				"Default" => "http://localhost:8080",
				"MyProject" => "http://localhost:8081",
				"TestProj" => "http://localhost:8082",
				_ => $"http://localhost:9000/{projectName.ToLower()}"
			};
		}

		public void Dispose()
		{
			_HTTP_CLIENT?.Dispose();
		}
	}
}