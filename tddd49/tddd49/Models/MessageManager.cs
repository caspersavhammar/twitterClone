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
    public class MessageManager{

        public struct message_template {
            public string Message { get; set; }
            public string From { get; set; }

            public message_template(string message, string from) {
                Message = message;
                From = from;
            }
        }

        public static message_template message_to_template(string message)
        {
            string[] contents = message.Split("Ω");
            return new message_template(contents[0], contents[1]);
        }
    }
};

