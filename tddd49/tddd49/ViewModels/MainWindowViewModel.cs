using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Reactive;
using System.Runtime.InteropServices;
using CommunityToolkit.Mvvm.ComponentModel;
using tddd49.Models;
using ReactiveUI;

namespace tddd49.ViewModels
{
    internal class MainWindowViewModel : INotifyPropertyChanged
    {
        NetworkManager network_manager;
        private string _ip_address;
        private string _port;
        public string ip_address
        {
            get { return _ip_address; } 
            set
            {
                _ip_address = value;
                OnPropertyChanged(nameof(ip_address));
            }
        }
        public string port
        {
            get { return _port; }
            set
            {
                _port = value;
                OnPropertyChanged(nameof(port));
            }
        }

        ReactiveCommand<Unit, Unit> connect;

        public MainWindowViewModel(NetworkManager nm)
        {
            network_manager = nm;
            connect = ReactiveCommand.Create(StartConnection);
        }

        private void StartConnection() {
            IPAddress casted_address = IPAddress.Parse(_ip_address);
            int casted_port = int.Parse(_port);
            Debug.Print(_port);
            Debug.Print(_ip_address);
            network_manager.startConnection(casted_address, casted_port);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }    
}
