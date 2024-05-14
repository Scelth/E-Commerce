using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Launcher.Services.Intefaces
{
    public interface IDownloadService
    {
        public string DownloadJson(string filePath);
    }
}
