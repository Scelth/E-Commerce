using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Launcher.Messages;
using Launcher.Model;
using Launcher.Services.Classes;
using Launcher.Services.Intefaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Launcher.ViewModel
{
    class StoreVM : ViewModelBase
    {
        private readonly IMessenger _messenger;
        private readonly IDownloadService _downloadService;
        private readonly INavigateService _navigateService;
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
        private ViewModelBase _info;
        public ViewModelBase Info
        {
            get => _info;
            set
            {
                Set(ref _info, value);
            }
        }

        public StoreVM(IMessenger messenger, IDownloadService downloadService, INavigateService navigateService, ISerializeService serializeService)
        {
            _messenger = messenger;
            _downloadService = downloadService;
            _navigateService = navigateService;
            _serializeService = serializeService;

            // Зарегистрируем, чтобы получить выбранный экземпляр модели проекта
            _messenger.Register<DataMessage>(this, message =>
            {
                ProjectModel = message.Data as ProjectModel;
            });

            CheckFile();
        }

        private string _selectedCategoryName;
        public string SelectedCategoryName
        {
            get => _selectedCategoryName;
            set
            {
                if (Set(ref _selectedCategoryName, value))
                {
                    UpdateSearchResult();
                }
            }
        }

        private void UpdateSearchResult()
        {
            string pathToCategoryFile = $@"..\Admin\Admin\bin\Debug\net6.0-windows\{SelectedCategoryName}Category.json";
            if (File.Exists(pathToCategoryFile))
            {
                string json = _downloadService.DownloadJson(pathToCategoryFile);
                var projects = JsonConvert.DeserializeObject<ObservableCollection<ProjectModel>>(json);
                SearchResult = new ObservableCollection<ProjectModel>(projects);
            }

            else
            {
                SearchResult = new ObservableCollection<ProjectModel>();
            }
        }

        public void CheckFile()
        {
            string pathToCategories = Check.pathToCategory;

            if (File.Exists(pathToCategories))
            {
                string json = _downloadService.DownloadJson(pathToCategories);
                var categories = _serializeService.Deserialize<ObservableCollection<CategoryModel>>(json);
                Data.Categories = new(categories);
            }
        }

        public RelayCommand<object> InfoCommand
        {
            get => new(name =>
            {
                string pathToCategoryFile = $@"..\Admin\Admin\bin\Debug\net6.0-windows\{SelectedCategoryName}Category.json";
                try
                {
                    if (name != null)
                    {
                        if (SelectedCategoryName != null)
                        {
                            string json = _downloadService.DownloadJson(pathToCategoryFile);
                            var projects = JsonConvert.DeserializeObject<ObservableCollection<ProjectModel>>(json);
                            var project = projects.FirstOrDefault(x => x.Name == (string)name);

                            Info = new InfoVM(_messenger, _serializeService)
                            { 
                                Project = project 
                            };
                        }

                        else
                        {
                            MessageBox.Show($"{SelectedCategoryName} category is empty", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Information", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            });
        }
    }
}
