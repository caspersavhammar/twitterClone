using System;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using tddd49.Views;

namespace tddd49.Models {
    internal class NetworkManager : INotifyPropertyChanged
    {
        private NetworkStream stream;
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName = "") {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));

            }
        }

        private string message;
        public string Message {
            get { return message; }
            set { message = value; OnPropertyChanged("Message"); }
        }

        public void startConnection(IPAddress address, int PORT) {

            var _alert_window = new AlertResponse();
            _alert_window.Show();
            Task.Factory.StartNew(() =>
            {
                bool secondTry = false;
                bool startNewWindow = false;
                IPEndPoint ipEndPoint = new IPEndPoint(address, PORT);
                TcpListener server = new TcpListener(ipEndPoint);
                TcpClient endPoint = null;
                try
                {
                    server.Start();
                    Console.WriteLine("Start listening...");
                    endPoint = server.AcceptTcpClient();
                    Console.WriteLine("Connection accepted!");
                    handleConnection(endPoint);

                }
                catch
                {
                    secondTry = true;
                }

                if (secondTry)
                {
                    endPoint = new TcpClient();
                    Console.WriteLine("we did it! yey!");
                    startNewWindow = true;
                    try
                    {
                        Console.WriteLine("Connecting to the server...");
                        endPoint.Connect(ipEndPoint);
                        Console.WriteLine("Connection established!");
                        handleConnection(endPoint);
                    }
                    finally
                    {
                        endPoint.Close();
                    }
                }

                // if (startNewWindow) {
                    // _alert_window.Show();
                // }
            });
            
        }

        private void handleConnection(TcpClient endPoint) {
            stream = endPoint.GetStream();
            while (true) {
                var buffer = new byte[1024];
                int received = stream.Read(buffer, 0, 1024);
                var message = Encoding.UTF8.GetString(buffer, 0, received);
                this.Message = message;
            }
        }
        public void sendChar(string str) {
            Task.Factory.StartNew(() => {
                var buffer = Encoding.UTF8.GetBytes(str);
                stream.Write(buffer, 0, str.Length);
            });
        }
    }
}
