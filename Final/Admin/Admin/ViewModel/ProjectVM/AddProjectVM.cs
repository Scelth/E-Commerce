using Admin.Messages;
using Admin.Model;
using Admin.Model.Services;
using Admin.Services.Interfaces;
using Admin.ViewModel.CategoryVM;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Admin.ViewModel.ProjectVM
{
    class AddProjectVM : ViewModelBase
    {
        public Data Data { get; set; } = new();

        private readonly IMessenger _messenger;
        private readonly ISerializeService _serializeService;
        private readonly IDownloadService _downloadService;

        private ProjectModel _projectModel = new();
        public ProjectModel ProjectModel
        {
            get => _projectModel;

            set
            {
                if (_projectModel != value)
                {
                    Set(ref _projectModel, value);
                }
            }
        }

        public void Check()
        {
            if (File.Exists("Categories.json"))
            {
                string json = _downloadService.DownloadJson("Categories.json");
                var categories = _serializeService.Deserialize<ObservableCollection<CategoryModel>>(json);
                Data.Categories = new(categories);
            }
        }

        public AddProjectVM(IMessenger messenger, ISerializeService serializeService, IDownloadService downloadService)
        {
            _messenger = messenger;
            _serializeService = serializeService;
            _downloadService = downloadService;

            _messenger.Register<DataMessage>(this, messenger =>
            {
                Data.Projects = messenger.Data as ObservableCollection<ProjectModel>;
            });

            Check();
        }

        public RelayCommand AddCommand
        {
            get => new(() =>
            {
                bool isModelValid = !string.IsNullOrWhiteSpace(ProjectModel.Name)
                && !string.IsNullOrWhiteSpace(ProjectModel.Studio)
                && ProjectModel.ReleaseDate != null
                && !string.IsNullOrWhiteSpace(ProjectModel.Price)
                && !string.IsNullOrWhiteSpace(ProjectModel.Poster)
                && !string.IsNullOrWhiteSpace(ProjectModel.Category)
                && !string.IsNullOrWhiteSpace(ProjectModel.Description);

                if (isModelValid)
                {
                    var json = _downloadService.DownloadJson($"{ProjectModel.Category}Category.json");

                    // If the json string is empty or null, create an empty array instead
                    if (string.IsNullOrEmpty(json))
                    {
                        json = "[]";
                    }

                    var projects = _serializeService.Deserialize<ObservableCollection<ProjectModel>>(json);

                    projects.Add(ProjectModel);

                    json = JsonConvert.SerializeObject(projects);
                    using FileStream fs = new($"{ProjectModel.Category}Category.json", FileMode.Create);
                    using StreamWriter sw = new(fs);
                    sw.Write(json);
                    MessageBox.Show($"{ProjectModel.Name} has been added to the {ProjectModel.Category} category", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                else
                {
                    MessageBox.Show("Fill in the required information!", "Information", MessageBoxButton.OK, MessageBoxImage.Hand);
                }
            });
        }
    }
}