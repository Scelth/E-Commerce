using Admin.Model.Messages;
using System;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Admin.Model.Services;
using Admin.ViewModel.CategoryVM;
using Admin.ViewModel.ProjectVM;

namespace Admin.ViewModel
{
    class MainVM : ViewModelBase
    {
        private ViewModelBase selectedPage;
        private readonly IMessenger _messenger;
        private readonly INavigateService _navigateService;

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
            _messenger = messenger;
            _navigateService = navigateService;
            _messenger.Register<NavigationMessage>(this, ReceiveMessage);
        }

        public RelayCommand AddCategoryCommand
        {
            get => new(() =>
            {
                _navigateService.NavigateTo<AddCategoryVM>();
            });
        }

        public RelayCommand EditCategoryCommand
        {
            get => new(() =>
            {
                _navigateService.NavigateTo<EditCategoryVM>();
            });
        }

        public RelayCommand DeleteCategoryCommand
        {
            get => new(() =>
            {
                _navigateService.NavigateTo<DeleteCategoryVM>();
            });
        }


        public RelayCommand AddProjectCommand
        {
            get => new(() =>
            {
                _navigateService.NavigateTo<AddProjectVM>();
            });
        }

        public RelayCommand SearchProjectCommand
        {
            get => new(() =>
            {
                _navigateService.NavigateTo<SearchProjectVM>();
            });
        }
    }
}