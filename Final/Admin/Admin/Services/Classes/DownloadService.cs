using Admin.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Services.Classes
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