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
        public string Greeting { get; } = "Welcome to Avalonia!";
        public static string address { get; set; }
        public static string port { get; set; }
        
        IPAddress casted_address = IPAddress.Parse(address);
        private int casted_port = Int16.Parse(port);

        public ReactiveCommand<Unit, Unit> start_connection() {
        
        }
    }    
}
