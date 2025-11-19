using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Reactive;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using tddd49.Models;
using ReactiveUI;

namespace tddd49.ViewModels
{
    internal class MainWindowViewModel : INotifyPropertyChanged
    {
        NetworkManager network_manager;
        private string _ip_address;
        private string _port;
        public string ip_address {
            get { return _ip_address; } 
            set {
                _ip_address = value;
                OnPropertyChanged(nameof(ip_address));
            }
        }
        public string port {
            get { return _port; }
            set {
                _port = value;
                OnPropertyChanged(nameof(port));
            }
        }

        public ICommand connect { get; }

        public MainWindowViewModel(NetworkManager nm) {
            network_manager = nm;
            connect = new RelayCommand(StartConnection);
        }

        private void StartConnection() {
            IPAddress casted_address = IPAddress.Parse(_ip_address);
            int casted_port = int.Parse(_port);
            Console.WriteLine(casted_address);
            Console.WriteLine(casted_port);
            network_manager.startConnection(casted_address, casted_port);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }    
}
