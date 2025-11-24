using System;
using System.ComponentModel;
using System.Net;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using tddd49.Models;

namespace tddd49.ViewModels
{
    internal class AlertResponseViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }    
}