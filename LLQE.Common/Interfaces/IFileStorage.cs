namespace LLQE.Common.Interfaces
{
    public interface IFileStorage
    {
        IEnumerable<string> GetDirectories(string? subfolder = null);
        IEnumerable<string> GetFiles(string folder);
        string? GetFileContent(string folder, string file);
        void DeleteFile(string folder, string file);
        void DeleteFolder(string folder);
    }
}
