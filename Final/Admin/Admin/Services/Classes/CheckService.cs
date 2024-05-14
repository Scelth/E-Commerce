using Admin.Model;
using Admin.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Admin.Services.Classes
{
    class CheckService : ICheckService
    {
        private IDownloadService _downloadService;
        private ISerializeService _serializeService;

        public bool CkeckCategoryExist(CategoryModel category)
        {
            foreach (var item in Data.Categories)
            {
                if (item.Name == category.Name)
                {
                    MessageBox.Show($"{item.Name} category already exist");
                    return false;
                }
            }
            return true;
        }
    }
}
