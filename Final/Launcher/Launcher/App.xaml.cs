using GalaSoft.MvvmLight.Messaging;
using Launcher.Services.Classes;
using Launcher.Services.Intefaces;
using Launcher.View;
using Launcher.ViewModel;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Launcher
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
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
            Container.RegisterSingleton<IUserManageService, UserManageService>();

            Container.RegisterSingleton<MainVM>();
            Container.RegisterSingleton<StoreVM>();
            Container.RegisterSingleton<InfoVM>();
            Container.RegisterSingleton<CartVM>();
            Container.RegisterSingleton<LogVM>();
            Container.RegisterSingleton<RegisterVM>();
            Container.RegisterSingleton<PayVM>();
            Container.RegisterSingleton<LibraryVM>();
        }

        private void MainStartup()
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.DataContext = Container.GetInstance<MainVM>();
            mainWindow.ShowDialog();
        }
    }
}
