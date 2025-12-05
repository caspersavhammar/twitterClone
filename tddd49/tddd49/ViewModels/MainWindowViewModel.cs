using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using System.Text.Json;
using System.IO;
using tddd49.Models;

namespace tddd49.ViewModels
{
    internal class MainWindowViewModel : INotifyPropertyChanged
    {
        public MainWindowViewModel(NetworkManager nm)
        {
            status_message = "";
            ip_address = "127.0.0.1";
            port = "13000";
            message_list =
                new ObservableCollection<MessageManager.message_template>(new List<MessageManager.message_template>());
            history_list = MessageManager.get_db();
            history_list_complete = MessageManager.get_db();
            network_manager = nm;
            network_manager.PropertyChanged += myModel_PropertyChanged;

            connect = new RelayCommand(Connect);
            start = new RelayCommand(Start);
            send_message_button = new RelayCommand(SendMessage);
            disconnect = new RelayCommand(Disconnect);
            show_conversation =
                new RelayCommand<ObservableCollection<MessageManager.message_template>>(ShowConversation);
        }

        public MainWindowViewModel() {}

        NetworkManager network_manager { get; set; }
        private string _ip_address;
        private string _port;
        private string _username;
        private string friends_username;
        private string _status_message;
        private string _send_message;
        private string _received_message;
        private string _search_history;

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

        public string username {
            get => _username;
            set {
                _username = value;
                network_manager.username = _username;
                OnPropertyChanged("username");
            }
        }
        
        public string status_message {
            get =>_status_message;
            set {
                _status_message = value;
                OnPropertyChanged("status_message");
            }
        }

        public string received_message {
            get => _received_message;
            set
            {
                _received_message = value;
                MessageManager.message_template message = MessageManager.MessageToTemplate(value);
                message_list.Add(message);
                OnPropertyChanged("received_message");
            }
        }

        public string send_message {
            get => _send_message;
            set {
                _send_message = value;
                OnPropertyChanged("send_message");
            }
        }
        
        public string search_history {
            get => _search_history;
            set {
                _search_history = value;
                history_list.Clear();
                foreach (var i in MessageManager.SearchHistory(history_list_complete, _search_history)) {
                    history_list.Add(i);
                }
                OnPropertyChanged("search_history");
            }
        }
        public ObservableCollection<MessageManager.message_template> message_list { get; set; }
        public ObservableCollection<Tuple<string, ObservableCollection<MessageManager.message_template>>> history_list { get; set; }
        private ObservableCollection<Tuple<string, ObservableCollection<MessageManager.message_template>>> history_list_complete { get; set; }
        
        public ICommand start { get; }
        public ICommand connect { get; }
        public ICommand send_message_button { get; }
        public ICommand disconnect { get; }
        public ICommand show_conversation { get; }
        private async void Start() {
            IPAddress casted_address = IPAddress.Parse(_ip_address);
            int casted_port = int.Parse(_port);
            await network_manager.StartConnection(casted_address, casted_port);
        }

        private async void Connect() {
            IPAddress casted_address = IPAddress.Parse(_ip_address);
            int casted_port = int.Parse(_port);
            await network_manager.Connect(casted_address, casted_port);
        }

        private async void SendMessage() {
                message_list.Add(MessageManager.MessageToTemplate(_send_message, _username));
                await network_manager.SendMessage(_send_message + "DEL" +  _username);
                send_message = "";
        }

        private void Disconnect()
        {
            MessageManager.SaveConversation(message_list, friends_username);
            network_manager.endPoint.Close();
        }

        private void ShowConversation(ObservableCollection<MessageManager.message_template>? convo)
        {
            MessageManager.SaveConversation(message_list, friends_username);
            message_list.Clear();
            foreach (var item in convo) {
                message_list.Add(item);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName = "") {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
   
        private void myModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e) {
            if (e.PropertyName == "status_message") {
                status_message = network_manager.status_message;
            } else if (e.PropertyName == "received_message") {
                received_message = network_manager.received_message;
            } else if (e.PropertyName == "friends_username") {
                friends_username = network_manager.friends_username;
            }
        }
    }
}
