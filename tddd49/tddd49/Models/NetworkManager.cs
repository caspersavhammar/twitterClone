using System;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Threading.Tasks;
using tddd49.Views;

namespace tddd49.Models {
    public class NetworkManager : INotifyPropertyChanged {
        public NetworkStream stream;
        public TcpClient endPoint;
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

            IPEndPoint ipEndPoint = null;
            TcpListener server = null;
            
            try {
                ipEndPoint = new IPEndPoint(address, PORT);
                server = new TcpListener(ipEndPoint);
            }
            catch {
                Connected_text = "Error: Need to specify address and PORT";
                return;
            }

            try {
                server.Start();
                Console.WriteLine("Start listening...");
                Connected_text = "Server started";
                while (true) {
                    endPoint = await server.AcceptTcpClientAsync();
                    var _alert_window = new AlertRequest(this);
                    _alert_window.Show();
                    stream = endPoint.GetStream();
                    if (await acceptConnection()) {
                        Connected_text = "Client connected";
                        _alert_window.Close();
                        break;
                    }

                    _alert_window.Close();
                }

                Console.WriteLine("Connection accepted!");
                await handleConnection().ConfigureAwait(false);
            }
            catch {
                Connected_text = "Error: Active server on port";
            }
            finally {
                server.Stop();
            }
        }

        public async Task connectConnection(IPAddress address, int PORT) {
            IPEndPoint ipEndPoint = null;
            
            try {
                ipEndPoint = new IPEndPoint(address, PORT);
                endPoint = new TcpClient();
            }
            catch {
                Connected_text = "Error: Need to specify address and PORT";
                return;
            }

            try {
                Console.WriteLine("Connecting to the server...");
                await endPoint.ConnectAsync(ipEndPoint);
                var _alert_window = new AlertResponse();
                _alert_window.Show();
                stream = endPoint.GetStream();
                if ( await acceptConnection()) {
                    var responseBytes = Encoding.UTF8.GetBytes("1");
                    await stream.WriteAsync(responseBytes);
                    Connected_text = "Server connected";
                    _alert_window.Close();
                }
                else {
                    _alert_window.Close();
                    Connected_text = "Error: No server on port";
                    endPoint.Close();
                    return;
                }
                Console.WriteLine("Connection established!");
                await handleConnection().ConfigureAwait(false);
            }
            finally {
                Console.WriteLine("If you sea this its to late");
                endPoint.Close();
            }
        }

        private async Task<bool> acceptConnection() {
            
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
            try {
                while (endPoint.Connected) {
                    var buffer = new byte[1_024];
                    int received = await stream.ReadAsync(buffer);
                    var message_from_stream = Encoding.UTF8.GetString(buffer, 0, received);
                    Console.WriteLine($"Message received: \"{message_from_stream}\"");
                    if (message_from_stream == "") {
                        throw new Exception("Error: No client connected");
                    }
                    this.Message = message_from_stream;
                }
            }
            catch (Exception e)
            {
                Connected_text = "Client disconnected";
                Console.WriteLine(e);
            }
        }
        public async Task sendChar(string str) {
            var buffer = Encoding.UTF8.GetBytes(str);
            try {
                await stream.WriteAsync(buffer, 0, str.Length);
            }
            catch {
                Connected_text = "Error: No established connection";
            }
        }
    }
}
