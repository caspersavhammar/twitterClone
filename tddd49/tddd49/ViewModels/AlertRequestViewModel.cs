using System.ComponentModel;
using System.Net.Sockets;
using System.Text;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using tddd49.Models;

namespace tddd49.ViewModels {
    public class AlertRequestViewModel : INotifyPropertyChanged {

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string property_name) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property_name));
        }

        NetworkManager network_manager { get; set; }

        public ICommand send_request { get; }

        public AlertRequestViewModel(NetworkManager nm) {
            network_manager = nm;
            send_request = new RelayCommand<string>(SendRequest);
        }

        public AlertRequestViewModel() {}

        private async void SendRequest(string response) {
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