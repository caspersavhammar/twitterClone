using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Threading.Tasks;
using tddd49.Views;

namespace tddd49.Models {
    public class MessageManager : INotifyPropertyChanged {
        
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName = "") {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public ObservableCollection<message_template> message_list { get; set; }
        public MessageManager()
        {
            message_list = new ObservableCollection<MessageManager.message_template>(new List<MessageManager.message_template>{
                new message_template("Meddelande1", "Din mamma", "Din pappa"),
                new message_template("Meddelande2", "Din mamma", "Din pappa"),
                new message_template("Meddelande3", "Din mamma", "Din pappa"),
                new message_template("Meddelande4", "Din mamma", "Din pappa"),
                new message_template("Jag är din far", "Gorbon", "Capo"),
            });
        }

        public void add_shit(string ett, string två, string tre)
        {
            message_list.Add(new message_template(ett, två, tre));
            Console.WriteLine(message_list.ToString());
        }
        public struct message_template {
            public string Message { get; set; }
            public string From { get; set; }
            public string To { get; set; }
            //public string Time { get; set; }

            public message_template(string message, string from, string to) {
                Message = message;
                From = from;
                To = to;
                //Time = time;
            }
        }

    }
};

