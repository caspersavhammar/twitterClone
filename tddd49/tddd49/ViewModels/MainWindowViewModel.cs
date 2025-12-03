using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Avalonia.Markup.Xaml.MarkupExtensions;
using CommunityToolkit.Mvvm.Input;
using tddd49.Models;

namespace tddd49.ViewModels
{
    internal class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName = "") {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        NetworkManager network_manager { get; set; }
        private string _ip_address;
        private string _port;
        private string _username;
        private string _connected_text;
        private string _message;

        public string ip_address {
            get => _ip_address;
            set {
                _ip_address = value;
                OnPropertyChanged("ip_address");
            }
        }
        public string port {
            get => _port;
            set {
                _port = value;
                OnPropertyChanged("port");
            }
        }

        public string username{
            get => _username;
            set {
                _username = value;
                OnPropertyChanged("username");
            }
        }
        
        public string connected_text{
            get =>_connected_text;
            set {
                _connected_text = value;
                OnPropertyChanged("connected_text");
            }
        }

        public string message {
            get => _message;
            set {
                _message = value;
                OnPropertyChanged("message");
            }
        }

        public ObservableCollection<MessageModel.message_template> message_list { get; set; }

        public ICommand connect { get; }
        public ICommand start { get; }
        public ICommand send_message { get; }
        public ICommand close_connection { get; }
        public ICommand choose_color { get; }

        public MainWindowViewModel(NetworkManager nm)
        {
            connected_text = "";
            network_manager = nm;
            network_manager.PropertyChanged += myModel_PropertyChanged;
            message_list = new ObservableCollection<MessageModel.message_template>(new List<MessageModel.message_template>{
                new MessageModel.message_template("Meddelande1", "Din mamma", "Din pappa"),
                new MessageModel.message_template("Meddelande2", "Din mamma", "Din pappa"),
                new MessageModel.message_template("Meddelande3", "Din mamma", "Din pappa"),
                new MessageModel.message_template("Meddelande4", "Din mamma", "Din pappa"),
                new MessageModel.message_template("Jag är din far", "Gorbon", "Capo"),
            });

            connect = new RelayCommand(ConnectConnection);
            start = new RelayCommand(StartConnection);
            send_message = new RelayCommand(send_char);
            close_connection = new RelayCommand(CloseConnection);
            choose_color = new RelayCommand(ChooseColor);
        }

        public MainWindowViewModel()
        {
            
        }

        private void myModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e) {
            if (e.PropertyName == "connected_text") {
                var text = network_manager.Connected_text;
                connected_text = text;
            }
        }

        private async void StartConnection() {
            IPAddress casted_address = IPAddress.Parse(_ip_address);
            int casted_port = int.Parse(_port);
            await network_manager.startConnection(casted_address, casted_port);
        }

        private async void ConnectConnection() {
            IPAddress casted_address = IPAddress.Parse(_ip_address);
            int casted_port = int.Parse(_port);
            await network_manager.connectConnection(casted_address, casted_port);
        }

        private async void send_char() {
                await network_manager.sendChar(_message);
        }

        private void CloseConnection()
        {
            network_manager.endPoint.Close();
        }

    }

}
