using Admin.Messages;
using Admin.Model;
using Admin.Model.Services;
using Admin.Services.Classes;
using Admin.Services.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace Admin.ViewModel.CategoryVM
{
    class DeleteCategoryVM : ViewModelBase
    {
        private readonly IMessenger _messenger;
        private readonly ISerializeService _serializeService;
        private readonly IDownloadService _downloadService;
        public Data Data { get; set; } = new();
        public CategoryModel categoryModel { get; set; } = new();

        private CategoryModel _categorytModel = new();
        public CategoryModel CategoryModel
        {
            get => _categorytModel;

            set
            {
                if (_categorytModel != value)
                {
                    Set(ref _categorytModel, value);
                }
            }
        }

        public DeleteCategoryVM(ISerializeService serializeService, IMessenger messenger, IDownloadService downloadService)
        {
            _messenger = messenger;
            _serializeService = serializeService;
            _downloadService = downloadService;

            _messenger.Register<DataMessage>(this, messenger =>
            {
                Data.Categories = messenger.Data as ObservableCollection<CategoryModel>;
            });

            CheckFile();
        }

        public void CheckFile()
        {
            string path = "Categories.json";
            if (File.Exists(path) && path.Length != 0)
            {
                string json = _downloadService.DownloadJson(path);
                var categories = _serializeService.Deserialize<ObservableCollection<CategoryModel>>(json);
                Data.Categories = new ObservableCollection<CategoryModel>(categories);
            }
        }

        public RelayCommand DeleteCommand
        {
            get => new(() =>
            {
                var categoryToRemove = Data.Categories.FirstOrDefault(x => x.Name == CategoryModel.Name); // Ищем категорию по названию
                if (categoryToRemove != null)
                {
                    File.Delete($"{CategoryModel.Name}Category.json");
                    MessageBox.Show($"{CategoryModel.Name} category deleted", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    Data.Categories.Remove(categoryToRemove);
                    string json = _serializeService.Serialize<Data>(Data.Categories);
                    File.WriteAllText("Categories.json", json);
                }
            });
        }
    }
}
