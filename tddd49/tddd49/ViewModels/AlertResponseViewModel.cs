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
        NetworkManager network_manager;
        private string _ip_address;
        private string _port;
        private string _username;
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

        public string username{
            get { return _username; }
            set {
                _port = value;
                OnPropertyChanged(nameof(_username));
            }
        }
        public ICommand connect { get; }

        public AlertResponseViewModel(NetworkManager nm) {
            network_manager = nm;
            connect = new RelayCommand(StartConnection);
        }

        public AlertResponseViewModel()
        {
            
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