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
using System.Windows.Controls;
using Launcher.Services.Classes;
using System.Windows;

namespace Launcher.ViewModel
{
    class RegisterVM : ViewModelBase
    {
        public UserModel User { get; set; } = new();

        private readonly INavigateService _navigateService;
        private readonly IUserManageService _userService;
        private readonly IMessenger _messenger;

        public RegisterVM(INavigateService navigateService, IUserManageService userService, IMessenger messenger)
        {
            _navigateService = navigateService;
            _userService = userService;
            _messenger = messenger;

            _messenger.Register<DataMessage>(this, message =>
            {
                User = message.Data as UserModel;
            });
        }

        public RelayCommand<object> RegisterCommand
        {
            get =>
                new(param =>
                {
                    if (param != null)
                    {
                        object[] res = (object[])param;

                        var password = (PasswordBox)res[0];
                        var confirm = (PasswordBox)res[1];

                        var checker = new PasswordService(password, confirm);

                        if (checker.IsMatch() && !_userService.CheckExists(User.Username, password.Password))
                        {
                            User.Password = password.Password;
                            User.ConfirmPassword = confirm.Password;

                            _userService.Add(User);
                        }

                        else
                        {
                            MessageBox.Show("The username or password is incorrect", "Notice", MessageBoxButton.OK, MessageBoxImage.Stop);
                        }

                        _navigateService.NavigateTo<LogVM>();
                    }
                });
        }
    }
}
