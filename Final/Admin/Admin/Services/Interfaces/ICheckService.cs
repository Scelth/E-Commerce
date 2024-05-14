using Admin.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Services.Interfaces
{
    interface ICheckService
    {
        public bool CkeckCategoryExist(CategoryModel category);
    }
}
