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
        
        public MessageManager() {}

        public struct message_template {
            public string Message { get; set; }
            public string From { get; set; }
            //public string Time { get; set; }

            public message_template(string message, string from) {
                Message = message;
                From = from;
                //Time = time;
            }
        }
    }
};

