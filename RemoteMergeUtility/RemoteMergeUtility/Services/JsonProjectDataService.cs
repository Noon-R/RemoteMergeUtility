using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using RemoteMergeUtility.Models;

namespace RemoteMergeUtility.Services
{
    public class JsonProjectDataService : IProjectDataService
    {
        private readonly string _FILE_PATH;

        public JsonProjectDataService()
        {
            var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var appFolderPath = Path.Combine(appDataPath, "RemoteMergeUtility");
            Directory.CreateDirectory(appFolderPath);
            _FILE_PATH = Path.Combine(appFolderPath, "projects.json");
        }

        public async Task<List<ProjectInfo>> LoadProjectsAsync()
        {
            try
            {
                if (!File.Exists(_FILE_PATH))
                {
                    return new List<ProjectInfo>();
                }

                var json = await File.ReadAllTextAsync(_FILE_PATH);
                var projects = JsonSerializer.Deserialize<List<ProjectInfo>>(json);
                return projects ?? new List<ProjectInfo>();
            }
            catch (Exception)
            {
                return new List<ProjectInfo>();
            }
        }

        public async Task SaveProjectsAsync(IEnumerable<ProjectInfo> projects)
        {
            try
            {
                var json = JsonSerializer.Serialize(projects, new JsonSerializerOptions
                {
                    WriteIndented = true
                });
                await File.WriteAllTextAsync(_FILE_PATH, json);
            }
            catch (Exception)
            {
                // ログ出力等の処理をここに追加可能
            }
        }
    }
}