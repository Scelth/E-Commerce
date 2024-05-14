using Admin.Messages;
using Admin.Model;
using Admin.Model.Services;
using Admin.Services.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace Admin.ViewModel.ProjectVM
{
    class EditProjectVM : ViewModelBase
    {
        private readonly IMessenger _messenger;
        private readonly ISerializeService _serializeService;
        private readonly INavigateService _navigateService;
        public Data Data { get; set; } = new();

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

        public EditProjectVM(IMessenger messenger, ISerializeService serializeService, INavigateService navigateService)
        {
            _navigateService = navigateService;
            _messenger = messenger;
            _serializeService = serializeService;

            _messenger.Register<DataMessage>(this, messenger =>
            {
                Data.Projects = messenger.Data as ObservableCollection<ProjectModel>;
            });
        }

        public RelayCommand EditCommand
        {
            get => new(() =>
            {
                var project = Data.Projects.FirstOrDefault(p => p.Name == ProjectModel.Name);
                if (project != null)
                {
                    //project.Name = ProjectModel.Name;
                    //project.Studio = ProjectModel.Studio;
                    //project.ReleaseDate = ProjectModel.ReleaseDate;
                    //project.Price = ProjectModel.Price;
                    //project.Poster = ProjectModel.Poster;
                    //project.Description = ProjectModel.Description;
                    //project.Category = ProjectModel.Category;

                    // Сохраняем изменения в JSON-файл
                    string fileName = $"{ProjectModel.Category}Category.json";
                    string filePath = Path.Combine(Environment.CurrentDirectory, fileName);
                    string json = _serializeService.Serialize<Data>(Data.Projects);
                    //File.WriteAllText(filePath, json);

                    using FileStream fs = new(filePath, FileMode.Open);
                    using StreamWriter sw = new(fs);
                    sw.Write(json);
                }
            });
        }
    }
}
