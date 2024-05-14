using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight;
using Launcher.Messages;
using Launcher.Services.Intefaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Launcher.Services.Classes
{
    internal class NavigateService : INavigateService
    {
        private readonly IMessenger _messenger;

        public NavigateService(IMessenger messenger)
        {
            _messenger = messenger;
        }

        public void NavigateTo<T>(object? data = null) where T : ViewModelBase
        {
            _messenger.Send(new NavigationMessage()
            {
                VMType = typeof(T)
            });

            if (data != null)
            {
                _messenger.Send(new DataMessage()
                {
                    Data = data
                });
            }
        }
    }
}
