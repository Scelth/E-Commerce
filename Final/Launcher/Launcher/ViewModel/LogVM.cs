using GalaSoft.MvvmLight.Views;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Launcher.Model;
using GalaSoft.MvvmLight.Command;
using Launcher.Services.Intefaces;
using System.Windows;
using Launcher.Messages;
using GalaSoft.MvvmLight.Messaging;
using Launcher.Services.Classes;
using System.Windows.Controls;

namespace Launcher.ViewModel
{
    internal class LogVM : ViewModelBase
    {
        public UserModel User { get; set; } = new();

        private readonly INavigateService _navigateService;
        private readonly IUserManageService _userService;
        private readonly IMessenger _messenger;
        public string Username { get; set; }

        public LogVM(INavigateService navigateService, IUserManageService userService, IMessenger messenger)
        {
            _navigateService = navigateService;
            _userService = userService;
            _messenger = messenger;

            _messenger.Register<DataMessage>(this, message =>
            {
                User = message.Data as UserModel;
            });
        }

        public RelayCommand<object> LoginCommand
        {
            get => new(
                param =>
                {
                    try
                    {
                        var password = param as PasswordBox;

                        var user = _userService.GetUser(Username, password.Password);

                        MessageBox.Show($"{user.Username} logged in");
                    }

                    catch (Exception ex)
                    {
                        MessageBox.Show("User doesn't exist");
                    }

                    Check.IsLoggedIn = true;
                    Check.Log = Username;
                    _navigateService.NavigateTo<StoreVM>();
                });
        }

        public RelayCommand RegisterCommand
        {
            get => new(() =>
                {
                    _navigateService.NavigateTo<RegisterVM>();
                });
        }
    }
}
