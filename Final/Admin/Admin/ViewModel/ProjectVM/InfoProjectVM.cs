using Admin.Messages;
using Admin.Model;
using Admin.Model.Services;
using Admin.Services.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace Admin.ViewModel.ProjectVM
{
    internal class InfoProjectVM : ViewModelBase
    {
        public ProjectModel Project { get; set; } = new();

        private readonly IMessenger _messenger;
        private readonly INavigateService _navigationService;
        private readonly IDownloadService _downloadService;
        private readonly ISerializeService _serializeService;

        public InfoProjectVM(IMessenger messenger, INavigateService navigationService, IDownloadService downloadService, ISerializeService serializeService)
        {
            _messenger = messenger;
            _navigationService = navigationService;
            _downloadService = downloadService;
            _serializeService = serializeService;

            // Зарегистрируем, чтобы получить выбранный экземпляр модели проекта
            _messenger.Register<DataMessage>(this, message =>
            {
                Project = message.Data as ProjectModel;
            });
        }

        public RelayCommand BackCommand
        {
            get => new(() =>
            {
                // Считываем данные из файла, чтобы передать в SearchProjectVM, ибо без этого багует
                string json = _downloadService.DownloadJson("Categories.json");
                var categories = _serializeService.Deserialize<ObservableCollection<CategoryModel>>(json);
                Data.Categories = new(categories);
                _navigationService.NavigateTo<SearchProjectVM>(Data.Categories);
            });
        }

        public RelayCommand EditCommand
        {
            get => new(() =>
            {
                _navigationService.NavigateTo<EditProjectVM>();
            });
        }

        public RelayCommand DeleteCommand
        {
            get => new(() =>
            {
                var json = _downloadService.DownloadJson($"{Project.Category}Category.json");
                var projects = JsonConvert.DeserializeObject<List<ProjectModel>>(json);

                // Удаляем выбранный проект по названию
                var projectToRemove = projects.FirstOrDefault(p => p.Name == Project.Name);
                if (projectToRemove != null)
                {
                    projects.Remove(projectToRemove);

                    // Записываем обновленные проекты обратно в файл JSON
                    json = JsonConvert.SerializeObject(projects);
                    File.WriteAllText($"{Project.Category}Category.json", json);
                }

                // Считываем данные из файла, чтобы передать в SearchProjectVM, ибо без этого багует
                string js = _downloadService.DownloadJson("Categories.json");
                var categories = _serializeService.Deserialize<ObservableCollection<CategoryModel>>(js);
                Data.Categories = new(categories);
                _navigationService.NavigateTo<SearchProjectVM>(Data.Categories);
            });
        }
    }
}