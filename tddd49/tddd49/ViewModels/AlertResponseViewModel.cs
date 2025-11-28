using System.ComponentModel;
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

        NetworkManager network_manager { get; set; }

        public ICommand sendResponse { get; }

        public AlertResponseViewModel(NetworkManager nm)
        {
            network_manager = nm;

            sendResponse = new RelayCommand(SendResponse);
        }

        public AlertResponseViewModel()
        {

        }

        public async void SendResponse()
        {

        }

    }    
}