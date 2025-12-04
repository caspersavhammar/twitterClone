using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;
using System.Windows.Input;
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
        MessageManager message_manager { get; set; }
        private string _ip_address;
        private string _port;
        private string _username;
        private string _connected_text;
        private string _send_message;
        private string _received_message;

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

        public string received_message {
            get => _received_message;
            set
            {
                _received_message = value;
                MessageManager.message_template message = MessageManager.message_to_template(value);
                message_list.Add(message);
                OnPropertyChanged("received_message");
            }
        }

        public string send_message
        {
            get => _send_message;
            set {
                _send_message = value;
                OnPropertyChanged("send_message");
            }
        }

        public ObservableCollection<MessageManager.message_template> message_list { get; set; }

        public ICommand connect { get; }
        public ICommand start { get; }
        public ICommand send_message_button { get; }
        public ICommand close_connection { get; }

        public MainWindowViewModel(NetworkManager nm, MessageManager mm) {
            connected_text = "";
            message_list = new ObservableCollection<MessageManager.message_template>(new List<MessageManager.message_template>());
            network_manager = nm;
            message_manager = mm;
            network_manager.PropertyChanged += myModel_PropertyChanged;

            connect = new RelayCommand(ConnectConnection);
            start = new RelayCommand(StartConnection);
            send_message_button = new RelayCommand(send_char);
            close_connection = new RelayCommand(CloseConnection);
        }

        public MainWindowViewModel() {}

        private void myModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e) {
            if (e.PropertyName == "connected_text") {
                connected_text = network_manager.Connected_text;
            } else if (e.PropertyName == "received_message") {
                received_message = network_manager.Received_message;
            }
        }

        private async void StartConnection()
        {
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
                message_list.Add(new MessageManager.message_template(_send_message, _username));
                await network_manager.sendChar(_send_message + ";" +  _username);
                send_message = "";
        }

        private void CloseConnection() {
            network_manager.endPoint.Close();
        }

    }

}
