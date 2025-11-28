using System;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using tddd49.Views;

namespace tddd49.Models {
    public class NetworkManager : INotifyPropertyChanged
    {
        public NetworkStream stream;
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

        private string connected_text;
        public string Connected_text {
            get { return connected_text; }
            set { connected_text = value; OnPropertyChanged("connected_text"); }
        }

        public async Task startConnection(IPAddress address, int PORT) {
            bool secondTry = false;
            IPEndPoint ipEndPoint = new IPEndPoint(address, PORT);
            TcpListener server = new TcpListener(ipEndPoint);
            TcpClient endPoint = null;
            try
            {
                server.Start();
                Console.WriteLine("Start listening...");
                Connected_text = "Server started";
                endPoint = await server.AcceptTcpClientAsync();
                Console.WriteLine(endPoint);
                var _alert_window = new AlertRequest(this);
                _alert_window.Show();
                stream = endPoint.GetStream();
                if ( await acceptConnection()) {
                    Connected_text = "Server connected";
                    _alert_window.Close();
                }
                else {
                    endPoint.Close();
                    return;
                }
                Console.WriteLine("Connection accepted!");
                await handleConnection().ConfigureAwait(false);
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
                    var _alert_window = new AlertResponse(this);
                    _alert_window.Show();
                    stream = endPoint.GetStream();
                    if ( await acceptConnection()) {
                        var responseBytes = Encoding.UTF8.GetBytes("1");
                        await stream.WriteAsync(responseBytes);
                        Connected_text = "Server connected";
                        _alert_window.Close();
                    }
                    else {
                        endPoint.Close();
                        return;
                    }
                    Console.WriteLine("Connection established!");
                    await handleConnection().ConfigureAwait(false);
                }
                finally
                {
                    Console.WriteLine("If you sea this its to late");
                    endPoint.Close();
                }
            }

        }

        private async Task<bool> acceptConnection()
        {
            
            var buffer = new byte[1_024];
            int received = await stream.ReadAsync(buffer);
            var message_from_stream = Encoding.UTF8.GetString(buffer, 0, received);
            Console.WriteLine($"Message received from accept window: \"{message_from_stream}\"");

            if (message_from_stream == "1") {
                return true;
            }

            return false;
        }

        private async Task handleConnection() {
            Console.WriteLine("We did it! yey. Handlin the connection");
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
        public async Task sendChar(string str) {
            var buffer = Encoding.UTF8.GetBytes(str);
            await stream.WriteAsync(buffer, 0, str.Length);
        }
    }
}
