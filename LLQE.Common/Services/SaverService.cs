using LLQE.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace LLQE.Common.Services
{
    public class SaverService : ISaver
    {
        private readonly string _fileStorage;

        public SaverService(IConfiguration configuration)
        {
            _fileStorage = configuration["FileStorage"];
        }

        public bool SaveResoponse(string responseName, string responeMessage)
        {
            

            return true;
        }
    }
}
