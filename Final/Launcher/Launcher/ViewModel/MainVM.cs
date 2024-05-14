using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight;
using Launcher.Messages;
using Launcher.Services.Intefaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using Launcher.Model;
using Launcher.Services.Classes;
using System.IO;

namespace Launcher.ViewModel
{
    class MainVM : ViewModelBase
    {
        private ViewModelBase selectedPage;
        private readonly IMessenger _messenger;
        private readonly INavigateService _navigateService;
        public UserModel User { get; set; }

        private string _logInButtonContent = "Log in";
        public string LogInButtonContent
        {
            get => _logInButtonContent;
            set
            {
                if (Check.IsLoggedIn)
                {
                    _logInButtonContent = "Profile";
                }

                else
                {
                    _logInButtonContent = "Log in";
                }
            }
        }

        public ViewModelBase CurrentViewModel
        {
            get => selectedPage;
            set
            {
                Set(ref selectedPage, value);
            }
        }

        public void ReceiveMessage(NavigationMessage message)
        {
            CurrentViewModel = App.Container.GetInstance(message.VMType) as ViewModelBase;
        }

        public MainVM(IMessenger messenger, INavigateService navigateService)
        {
            User = new();
            CurrentViewModel = App.Container.GetInstance<StoreVM>(); // Ставлю окно по умолчанию 

            _messenger = messenger;
            _navigateService = navigateService;
            _messenger.Register<NavigationMessage>(this, ReceiveMessage);
        }

        public RelayCommand StoreCommand
        {
            get => new(() =>
            {
                _navigateService.NavigateTo<StoreVM>();
            });
        }

        public RelayCommand CartCommand
        {
            get => new(() =>
            {
                _navigateService.NavigateTo<CartVM>();
            });
        }

        public RelayCommand LogCommand
        {
            get => new(() =>
            {
                _navigateService.NavigateTo<LogVM>();
            });
        }

        public RelayCommand LibraryCommand
        {
            get => new(() =>
            {
                if(Check.IsLoggedIn)
                {
                    _navigateService.NavigateTo<LibraryVM>();
                }

                else
                {
                    _navigateService.NavigateTo<LogVM>();
                }
            });
        }
    }
}
