using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Launcher.Services.Classes
{
    public static class Check
    {
        public static bool IsLoggedIn { set; get; }
        public static string Log { set; get; }

        public static string pathToCategory = @"..\Admin\Admin\bin\Debug\net6.0-windows\Categories.json";
    }
}