using LLQE.Common.Interfaces;
using Microsoft.Extensions.Configuration;

namespace LLQE.Common.Services
{
    public class SaverService : ISaver
    {
        private readonly string _fileStorage;

        public SaverService(IConfiguration configuration)
        {
            _fileStorage = configuration["FileStorage"];
        }

        public bool SaveResoponse(string folderName, string fileName, string responseMessage)
        {
            try
            {
                fileName = fileName + ".txt";

                var directoryPath = Path.Combine(_fileStorage, folderName);

                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                var filePath = Path.Combine(directoryPath, fileName);

                if (File.Exists(filePath))
                {
                    var fileNameWithoutExt = Path.GetFileNameWithoutExtension(fileName);
                    var extension = Path.GetExtension(fileName);
                    var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                    var newFileName = $"{fileNameWithoutExt}_{timestamp}{extension}";

                    filePath = Path.Combine(directoryPath, newFileName);
                }

                File.WriteAllText(filePath, responseMessage);

                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}
