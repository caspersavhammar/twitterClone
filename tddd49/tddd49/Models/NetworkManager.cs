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

        public async Task startConnection(IPAddress address, int PORT) {
            var _alert_window = new AlertResponse();
            _alert_window.Show();
            bool secondTry = false;
            IPEndPoint ipEndPoint = new IPEndPoint(address, PORT);
            TcpListener server = new TcpListener(ipEndPoint);
            TcpClient endPoint = null;
            try
            {
                server.Start();
                Console.WriteLine("Start listening...");
                endPoint = await server.AcceptTcpClientAsync();
                
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
                try
                {
                    Console.WriteLine("Connecting to the server...");
                    await endPoint.ConnectAsync(ipEndPoint);
                    Console.WriteLine("Connection established!");
                    handleConnection(endPoint);
                }
                finally
                {
                    endPoint.Close();
                }
            }

        }

        private async Task handleConnection(TcpClient endPoint) {
            Console.WriteLine("We did it! yey. Handlin the connection");
            stream = endPoint.GetStream();
            Console.WriteLine("We got the stream");
            
            while (true) {
                var buffer = new byte[1_024];
                Console.WriteLine("We stuck here bish");
                int received = await stream.ReadAsync(buffer);
                var message_from_stream = Encoding.UTF8.GetString(buffer, 0, received);
                Console.WriteLine($"Message received: \"{message_from_stream}\"");
                this.Message = message_from_stream;
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
