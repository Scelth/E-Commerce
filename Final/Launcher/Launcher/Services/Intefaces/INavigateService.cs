using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Launcher.Services.Intefaces
{
    public interface INavigateService
    {
        public void NavigateTo<T>(object? data = null) where T : ViewModelBase;
    }
}
