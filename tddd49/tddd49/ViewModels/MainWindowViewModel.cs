using System;
using System.ComponentModel;
using System.Net;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using tddd49.Models;

namespace tddd49.ViewModels
{
    internal class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        
        NetworkManager network_manager;
        private string _ip_address;
        private string _port;
        private string _username;
        private string _connected_text;
        public string ip_address {
            get => _ip_address;
            set {
                _ip_address = value;
                OnPropertyChanged();
            }
        }
        public string port {
            get => _port;
            set {
                _port = value;
                OnPropertyChanged();
            }
        }

        public string username{
            get => _username;
            set {
                _username = value;
                OnPropertyChanged();
            }
        }
        
        public string connected_text{
            get =>_connected_text;
            set {
                _connected_text = value;
                OnPropertyChanged();
            }
        }
        public ICommand connect { get; }

        public MainWindowViewModel(NetworkManager nm)
        {
            connected_text = "";
            network_manager = nm;
            connect = new RelayCommand(StartConnection);
        }

        public MainWindowViewModel()
        {
            
        }

        private async void StartConnection() {
            IPAddress casted_address = IPAddress.Parse(_ip_address);
            int casted_port = int.Parse(_port);
            connected_text = "Server started";
            await network_manager.startConnection(casted_address, casted_port);
        }

        public void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }    
}
