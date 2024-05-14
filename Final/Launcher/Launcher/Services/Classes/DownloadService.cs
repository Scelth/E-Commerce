using Launcher.Services.Intefaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Launcher.Services.Classes
{
    public class DownloadService : IDownloadService
    {
        public string DownloadJson(string filePath)
        {
            try
            {
                using StreamReader reader = new(filePath);
                string jsonContent = reader.ReadToEnd();
                return jsonContent;
            }

            catch (Exception)
            {
                throw;
            }
        }
    }
}
