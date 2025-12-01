using System;
using System.ComponentModel;
using System.Net.Sockets;
using System.Text;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using tddd49.Models;

namespace tddd49.ViewModels
{
    public class AlertRequestViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        NetworkManager network_manager { get; set; }

        public ICommand sendRequest { get; }

        public AlertRequestViewModel(NetworkManager nm)
        {
            network_manager = nm;
            sendRequest = new RelayCommand<string>(SendRequest);
        }

        public AlertRequestViewModel()
        {
        }

        private async void SendRequest(string response)
        {
            NetworkStream stream = network_manager.stream;
            if (response == "1") {
                var responseBytes = Encoding.UTF8.GetBytes(response);
                await stream.WriteAsync(responseBytes);
            }
            else if (response == "0") {
                var responseBytes = Encoding.UTF8.GetBytes(response);
                await stream.WriteAsync(responseBytes);
            }
        }
    }
}