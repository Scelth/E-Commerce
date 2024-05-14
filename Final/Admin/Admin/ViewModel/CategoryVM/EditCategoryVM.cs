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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Admin.ViewModel.CategoryVM
{
    class EditCategoryVM : ViewModelBase
    {
        private readonly IMessenger _messenger;
        private readonly ISerializeService _serializeService;
        public Data Data { get; set; } = new();

        private CategoryModel _categoryModel = new();
        public CategoryModel CategoryModel
        {
            get => _categoryModel;

            set
            {
                if (_categoryModel != value)
                {
                    Set(ref _categoryModel, value);
                }
            }
        }

        public EditCategoryVM(IMessenger messenger, ISerializeService serializeService)
        {
            _messenger = messenger;
            _serializeService = serializeService;

            _messenger.Register<DataMessage>(this, messenger =>
            {
                Data.Categories = messenger.Data as ObservableCollection<CategoryModel>;
            });
        }

        public RelayCommand EditCommand
        {
            get => new(() =>
            {
                if (CategoryModel.NewName != null)
                {
                    if (!File.Exists($"{CategoryModel.NewName}Category.json"))
                    {
                        CategoryModel category = Data.Categories.FirstOrDefault(x => x.Name == CategoryModel.Name);
                        if (category != null)
                        {
                            string temp = category.Name;
                            category.Name = CategoryModel.NewName;

                            string path = $"{category.Name}Category.json";

                            // Переименовываем файл
                            File.Move($"{temp}Category.json", path);

                            // Обновляем название категории в каждом проекте внутри файла
                            string fileContents = File.ReadAllText(path);

                            var projects = JsonConvert.DeserializeObject<List<ProjectModel>>(fileContents);

                            if (projects != null)
                            {
                                foreach (var project in projects)
                                {
                                    project.Category = category.Name;
                                }
                            }

                            string updatedFileContents = JsonConvert.SerializeObject(projects);
                            File.WriteAllText(path, updatedFileContents);

                            // Обновляем название категории в объекте данных
                            int index = Data.Categories.IndexOf(category);
                            Data.Categories.RemoveAt(index);
                            Data.Categories.Insert(index, category);

                            // Сериализуем и записываем обновленный объект данных в Categories.json
                            string json = _serializeService.Serialize<Data>(Data.Categories);
                            using FileStream fs = new("Categories.json", FileMode.Open);
                            using StreamWriter sw = new(fs);
                            sw.Write(json);

                            MessageBox.Show($"{temp} category changed to {category.Name}", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }

                    else
                    {
                        MessageBox.Show($"{CategoryModel.NewName} category already exists", "Information", MessageBoxButton.OK, MessageBoxImage.Stop);
                    }
                }

                else
                {
                    MessageBox.Show("Enter the category name!", "Information", MessageBoxButton.OK, MessageBoxImage.Hand);
                }
            });
        }
    }
}
