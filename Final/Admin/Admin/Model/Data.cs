using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Model
{
    public class Data
    {
        public static ObservableCollection<CategoryModel> Categories { get; set; } = new();
        public static ObservableCollection<ProjectModel> Projects { get; set; } = new();
    }
}