using System;
using System.Net;
using System.Reactive;
using CommunityToolkit.Mvvm.ComponentModel;
using tddd49.Models;
using ReactiveUI; 

namespace tddd49.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        
        [ObservableProperty] private string _title;
        NetworkManager networkManager;
        public ReactiveCommand<IPAddress, Unit> address {
            get {
                return casted_address;
            }
            set {
                IPAddress casted_address = IPAddress.Parse(address);
                // Update notifier
            }
        }

        private IPAddress casted_address;
        public ReactiveCommand<Unit, Unit> port { get; set; }
        
        private int casted_port = Int16.Parse(port);

        public MainWindowViewModel()
        {
            Connect = ReactiveCommand.Create(StartConnection);
        }

        private void StartConnection() {
            networkManager.startConnection(casted_address, casted_port);
        }
    }    
}
