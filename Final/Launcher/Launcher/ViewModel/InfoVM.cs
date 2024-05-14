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
using System.IO;
using System.Windows;
using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace Launcher.ViewModel
{
    class InfoVM : ViewModelBase
    {
        public ProjectModel Project { get; set; } = new();

        private readonly IMessenger _messenger;
        private readonly ISerializeService _serializeService;

        public InfoVM(IMessenger messenger, ISerializeService serializeService)
        {
            _messenger = messenger;
            _serializeService = serializeService;

            // Зарегистрируем, чтобы получить выбранный экземпляр модели проекта
            _messenger.Register<DataMessage>(this, message =>
            {
                Project = message.Data as ProjectModel;
            });
        }

        public RelayCommand CartCommand
        {
            get => new(() =>
            {
                using FileStream fs = new("Cart.json", FileMode.OpenOrCreate);
                using StreamReader sr = new(fs);
                using StreamWriter sw = new(fs);

                string json = sr.ReadToEnd();
                var cart = JsonConvert.DeserializeObject<List<ProjectModel>>(json) ?? new List<ProjectModel>();

                var existingProject = cart.FirstOrDefault(x => x.Name == Project.Name);

                if (existingProject == null)
                {
                    cart.Add(Project);
                    json = JsonConvert.SerializeObject(cart);

                    sw.BaseStream.SetLength(0); // Очищаем файл
                    sw.Write(json);

                    MessageBox.Show($"{Project.Name} has been added to the cart", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                else
                {
                    MessageBox.Show($"{Project.Name} is already in the cart", "Information", MessageBoxButton.OK, MessageBoxImage.Stop);
                }
            });
        }
    }
}