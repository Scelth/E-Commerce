using Admin.Model.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using SimpleInjector;
using System.Windows.Navigation;
using Admin.ViewModel;
using Admin.Services.Classes;
using Admin.Services.Interfaces;
using Admin.ViewModel.CategoryVM;
using Admin.ViewModel.ProjectVM;

namespace Admin
{
    public partial class App : Application
    {
        public static Container Container { get; set; } = new();

        protected override void OnStartup(StartupEventArgs e)
        {
            Register();
            MainStartup();
        }

        private void Register()
        {
            Container.RegisterSingleton<IMessenger, Messenger>();
            Container.RegisterSingleton<INavigateService, NavigateService>();
            Container.RegisterSingleton<IDownloadService, DownloadService>();
            Container.RegisterSingleton<ISerializeService, SerializeService>();
            Container.RegisterSingleton<ICheckService, CheckService>();

            Container.RegisterSingleton<MainVM>();

            Container.RegisterSingleton<AddCategoryVM>();
            Container.RegisterSingleton<EditCategoryVM>();
            Container.RegisterSingleton<DeleteCategoryVM>();

            Container.RegisterSingleton<AddProjectVM>();
            Container.RegisterSingleton<SearchProjectVM>();
            Container.RegisterSingleton<InfoProjectVM>();
            Container.RegisterSingleton<EditProjectVM>();
        }

        private void MainStartup()
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.DataContext = Container.GetInstance<MainVM>();
            mainWindow.ShowDialog();
        }
    }
}