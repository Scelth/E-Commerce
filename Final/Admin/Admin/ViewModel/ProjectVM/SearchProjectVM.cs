using Admin.Messages;
using Admin.Model;
using Admin.Model.Services;
using Admin.Services.Classes;
using Admin.Services.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MaterialDesignColors;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.DirectoryServices;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Admin.ViewModel.ProjectVM
{
    class SearchProjectVM : ViewModelBase
    {
        private readonly IMessenger _messenger;
        private readonly INavigateService _navigateService;
        private readonly IDownloadService _downloadService;
        public string SearchBar { get; set; }
        public Data Data { get; set; } = new();

        public ProjectModel ProjectModel { get; set; } = new();

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

        public SearchProjectVM(INavigateService navigationService, IMessenger messenger, IDownloadService downloadService)
        {
            _messenger = messenger;
            _navigateService = navigationService;
            _downloadService = downloadService;

            // Зарегистрируем, чтобы получить выбранный экземпляр модели проекта
            _messenger.Register<DataMessage>(this, message =>
            {
                ProjectModel = message.Data as ProjectModel;
            });
        }

        private string _selectedCategoryName;
        public string SelectedCategoryName
        {
            get => _selectedCategoryName;
            set
            {
                Set(ref _selectedCategoryName, value);
            }
        }

        public RelayCommand SearchCommand
        {
            get => new(() =>
            {
                SearchResult.Clear();
                if (SelectedCategoryName != null)
                {
                    string json = _downloadService.DownloadJson($"{SelectedCategoryName}Category.json");
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

                    else
                    {
                        MessageBox.Show($"{SelectedCategoryName} category is empty", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }

                else
                {
                    MessageBox.Show("Please select a category", "Notice", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            });
        }

        public RelayCommand<object> InfoCommand
        {
            get => new(name =>
            {
                try
                {
                    if (name != null)
                    {
                        if (SelectedCategoryName != null)
                        {
                            string json = _downloadService.DownloadJson($"{SelectedCategoryName}Category.json");
                            var projects = JsonConvert.DeserializeObject<ObservableCollection<ProjectModel>>(json);
                            var project = projects.FirstOrDefault(x => x.Name == (string)name);
                            _navigateService.NavigateTo<InfoProjectVM>(project);
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