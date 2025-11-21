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

        async public void startConnection(IPAddress address, int PORT) {
            Console.WriteLine("start connection is starting on the net work yeeees");

            Task.Factory.StartNew(() =>
            {
                bool secondTry = false;
                IPEndPoint ipEndPoint = new IPEndPoint(address, PORT);
                TcpListener server = new TcpListener(ipEndPoint);
                TcpClient endPoint = null;
                try
                {
                    Console.WriteLine("Starting server on the main bich yeeeees...");
                    server.Start();
                    System.Diagnostics.Debug.WriteLine("Start listening...");
                    endPoint = server.AcceptTcpClient();
                    System.Diagnostics.Debug.WriteLine("Connection accepted!");
                    handleConnection(endPoint);

                }
                catch
                {
                    secondTry = true;
                }

                if (secondTry)
                {
                    Console.WriteLine("Connecting to main bich yuh");
                    endPoint = new TcpClient();
                    try
                    {
                        AlertResponse _alert_window = new AlertResponse();
                        //await () async => _alert_window.ShowDialog(this);
                        System.Diagnostics.Debug.WriteLine("Connecting to the server...");
                        endPoint.Connect(ipEndPoint);
                        System.Diagnostics.Debug.WriteLine("Connection established!");
                        handleConnection(endPoint);
                    }
                    finally
                    {
                        endPoint.Close();
                    }
                }
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
