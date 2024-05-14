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
using GalaSoft.MvvmLight.Command;
using Launcher.Services.Classes;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;

namespace Launcher.ViewModel
{
    public class PayVM : ViewModelBase
    {
        public ProjectModel Project { get; set; } = new();
        public UserModel User { get; set; } = new();
        public Data Data { get; set; } = new();
        public int Total { set; get; }

        private readonly IMessenger _messenger;
        private readonly ISerializeService _serializeService;
        private readonly IDownloadService _downloadService;
        private readonly INavigateService _navigateService;

        public PayVM(IMessenger messenger, ISerializeService serializeService, IDownloadService downloadService, INavigateService navigateService)
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
        }


        public RelayCommand PayCommand
        {
            get => new(() =>
            {
                string cartPath = "Cart.json";
                string libraryPath = $"{Check.Log}Library.json";

                // Создаем новый файл и копируем в него содержимое из "Cart.json"
                File.Copy(cartPath, libraryPath, true);

                // Загружаем содержимое нового файла
                string json = _downloadService.DownloadJson(libraryPath);
                var projects = _serializeService.Deserialize<ObservableCollection<ProjectModel>>(json);
                Data.Projects = new ObservableCollection<ProjectModel>(projects);

                MessageBox.Show("Good");

                // Удаляем файл "Cart.json"
                File.Delete(cartPath);

                _navigateService.NavigateTo<StoreVM>();
            });
        }
    }
}
