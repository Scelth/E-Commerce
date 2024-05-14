using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight;
using Launcher.Messages;
using Launcher.Model;
using Launcher.Services.Intefaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using System.IO;
using Launcher.Services.Classes;
using System.Windows;
using System.ComponentModel;

namespace Launcher.ViewModel
{
    internal class LibraryVM : ViewModelBase
    {
        private readonly IMessenger _messenger;
        private readonly IDownloadService _downloadService;
        private readonly ISerializeService _serializeService;
        public Data Data { get; set; } = new();
        public ProjectModel ProjectModel { get; set; } = new();
        public string SearchBar { get; set; }

        private ObservableCollection<ProjectModel> _searchResult = new();
        public ObservableCollection<ProjectModel> SearchResult
        {
            get => _searchResult;
            set
            {
                if (_searchResult != value)
                {
                    Set(ref _searchResult, value);
                }
            }
        }

        public LibraryVM(IMessenger messenger, IDownloadService downloadService, ISerializeService serializeService)
        {
            _messenger = messenger;
            _downloadService = downloadService;
            _serializeService = serializeService;

            // Зарегистрируем, чтобы получить выбранный экземпляр модели проекта
            _messenger.Register<DataMessage>(this, message =>
            {
                ProjectModel = message.Data as ProjectModel;
            });

            //PropertyChanged += LibraryVM_PropertyChanged;
            CheckLab();
        }

        private void LibraryVM_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SearchBar))
            {
                // Вызываем метод Lab() для обновления SearchResult
                SearchResult.Clear();
                CheckLab();
            }
        }

        public void CheckLab()
        {
            string path = $"{Check.Log}Library.json";

            if (File.Exists(path))
            {
                string json = _downloadService.DownloadJson(path);
                var projects = JsonConvert.DeserializeObject<ObservableCollection<ProjectModel>>(json);
                SearchResult = new ObservableCollection<ProjectModel>(projects);
            }
        }

        public void Lab()
        {
            string json = _downloadService.DownloadJson($"{Check.Log}Library.json");
            var projects = JsonConvert.DeserializeObject<ObservableCollection<ProjectModel>>(json);

            if (projects != null && projects.Count > 0)
            {
                if (SearchBar != null) // Если мы ввели что-то в SearchBar
                {
                    foreach (var project in projects)
                    {
                        if (project.Name == SearchBar) // Проверяем, совпадает ли название проекта с текстом в SearchBar
                        {
                            SearchResult.Add(project);
                        }
                    }
                }

                else // Если мы не ввели что-то в SearchBar
                {
                    foreach (var project in projects)
                    {
                        SearchResult.Add(project);
                    }
                }
            }
        }
    }
}