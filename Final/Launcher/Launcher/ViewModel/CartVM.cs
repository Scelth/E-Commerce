using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight;
using Launcher.Messages;
using Launcher.Model;
using Launcher.Services.Intefaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Launcher.Services.Classes;
using System.Collections.ObjectModel;
using System.IO;
using GalaSoft.MvvmLight.Command;
using System.Windows;

namespace Launcher.ViewModel
{
    class CartVM : ViewModelBase
    {
        public ProjectModel Project { get; set; } = new();
        public UserModel User { get; set; } = new();
        public Data Data { get; set; } = new();
        public int Total { set; get; }

        private readonly IMessenger _messenger;
        private readonly ISerializeService _serializeService;
        private readonly IDownloadService _downloadService;
        private readonly INavigateService _navigateService;

        public CartVM(IMessenger messenger, ISerializeService serializeService, IDownloadService downloadService, INavigateService navigateService)
        {
            _messenger = messenger;
            _serializeService = serializeService;
            _downloadService = downloadService;
            _navigateService = navigateService;

            // Зарегистрируем, чтобы получить выбранный экземпляр модели проекта
            _messenger.Register<DataMessage>(this, message =>
            {
                Project = message.Data as ProjectModel;
            });

            Cart();
        }

        public void Cart()
        {
            string path = "Cart.json";
            if (File.Exists(path) && path.Length != 0)
            {
                string json = _downloadService.DownloadJson(path);
                var projects = _serializeService.Deserialize<ObservableCollection<ProjectModel>>(json);
                Data.Projects = new ObservableCollection<ProjectModel>(projects);
                UpdateTotal();
            }
        }

        public void UpdateTotal()
        {
            Total += Data.Projects.Sum(x => Int32.Parse(x.Price));
        }

        public RelayCommand PayCommand
        {
            get => new(() =>
            {
                if (Check.IsLoggedIn)
                {
                    _navigateService.NavigateTo<PayVM>();
                }

                else
                {
                    _navigateService.NavigateTo<LogVM>();
                }
            });
        }
    }
}
