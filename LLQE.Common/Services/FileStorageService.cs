using Microsoft.Extensions.Configuration;

namespace LLQE.Common.Services
{
    public class FileStorageService
    {
        private readonly string _rootPath;

        public FileStorageService(IConfiguration config)
        {
            _rootPath = config["FileStorage"];
        }

        public IEnumerable<string> GetDirectories(string? subfolder = null)
        {
            var path = string.IsNullOrEmpty(subfolder) ? _rootPath : Path.Combine(_rootPath, subfolder);
            if (!Directory.Exists(path)) return Enumerable.Empty<string>();
            return Directory.GetDirectories(path).Select(Path.GetFileName);
        }

        public IEnumerable<string> GetFiles(string folder)
        {
            var path = Path.Combine(_rootPath, folder);
            if (!Directory.Exists(path)) return Enumerable.Empty<string>();
            return Directory.GetFiles(path).Select(Path.GetFileName);
        }

        public string? GetFileContent(string folder, string file)
        {
            var path = Path.Combine(_rootPath, folder, file);
            return File.Exists(path) ? File.ReadAllText(path) : null;
        }

        public void DeleteFile(string folder, string file)
        {
            var path = Path.Combine(_rootPath, folder, file);
            if (File.Exists(path)) File.Delete(path);
        }

        public void DeleteFolder(string folder)
        {
            var path = Path.Combine(_rootPath, folder);
            if (Directory.Exists(path)) Directory.Delete(path, true);
        }
    }
}
