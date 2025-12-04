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
        public string username;
        
        private string _status_message;
        private string _received_message;
        private string _friends_username;

        public string status_message {
            get => _status_message;
            private set { _status_message = value; OnPropertyChanged("status_message"); }
        }

        public string received_message {
            get => _received_message;
            private set { _received_message = value; OnPropertyChanged("received_message");}
        }

        public string friends_username
        {
            get => _friends_username;
            set { _friends_username= value; OnPropertyChanged("friends_username"); }
        }

        public async Task StartConnection(IPAddress address, int PORT) {

            IPEndPoint ipEndPoint = null;
            TcpListener server = null;
            
            try {
                ipEndPoint = new IPEndPoint(address, PORT);
                server = new TcpListener(ipEndPoint);
            }
            catch {
                status_message = "Error: Need to specify address and PORT";
                return;
            }

            try {
                server.Start();
                Console.WriteLine("Start listening...");
                status_message = "Server started";
                while (true) {
                    endPoint = await server.AcceptTcpClientAsync();
                    var _alert_window = new AlertRequest(this);
                    _alert_window.Show();
                    stream = endPoint.GetStream();
                    if (await AcceptConnection()) {
                        status_message = "Client connected";
                        _alert_window.Close();
                        
                        // Send username to Server:
                        var usernameBytes = Encoding.UTF8.GetBytes(username);
                        await stream.WriteAsync(usernameBytes);
                        // Receive username from Server:
                        var buffer = new byte[1_024];
                        int received_username = await stream.ReadAsync(buffer);
                        friends_username = Encoding.UTF8.GetString(buffer, 0, received_username);
                        
                        break;
                    }

                    _alert_window.Close();
                }

                Console.WriteLine("Connection accepted!");
                await HandleConnection().ConfigureAwait(false);
            }
            catch {
                status_message = "Error: Active server on port";
            }
            finally {
                server.Stop();
            }
        }

        public async Task Connect(IPAddress address, int PORT) {
            IPEndPoint ipEndPoint = null;
            
            try {
                ipEndPoint = new IPEndPoint(address, PORT);
            }
            catch {
                status_message = "Error: Need to specify address and PORT";
                return;
            }

            try {
                endPoint = new TcpClient();
                Console.WriteLine("Connecting to the server...");
                await endPoint.ConnectAsync(ipEndPoint);
                var _alert_window = new AlertResponse();
                _alert_window.Show();
                stream = endPoint.GetStream();
                if ( await AcceptConnection()) {
                    var responseBytes = Encoding.UTF8.GetBytes("1");
                    await stream.WriteAsync(responseBytes);
                    
                    status_message = "Server connected";
                    _alert_window.Close();

                    // Send username to Server:
                    var usernameBytes = Encoding.UTF8.GetBytes(username);
                    await stream.WriteAsync(usernameBytes);
                    // Receive username from Server:
                    var buffer = new byte[1_024];
                    int received_username = await stream.ReadAsync(buffer);
                    friends_username = Encoding.UTF8.GetString(buffer, 0, received_username);
                }
                else {
                    _alert_window.Close();
                    status_message = "Error: No server on port";
                    endPoint.Close();
                    return;
                }
                Console.WriteLine("Connection established!");
                await HandleConnection().ConfigureAwait(false);
            }
            finally {
                Console.WriteLine("If you sea this its to late");
                endPoint.Close();
            }
        }

        private async Task<bool> AcceptConnection() {
            
            var buffer = new byte[1_024];
            int received = await stream.ReadAsync(buffer);
            var message_from_stream = Encoding.UTF8.GetString(buffer, 0, received);

            if (message_from_stream == "1") {
                return true;
            }

            return false;
        }

        private async Task HandleConnection() {
            try {
                while (true) {
                    var buffer = new byte[1_024];
                    int received = await stream.ReadAsync(buffer);
                    var message_from_stream = Encoding.UTF8.GetString(buffer, 0, received);
                    if (message_from_stream == "") {
                        throw new Exception("Error: No client connected");
                    }
                    received_message = message_from_stream;
                }
            }
            catch (Exception e) {
                status_message = "Client disconnected";
                Console.WriteLine(e);
            }
        }
        public async Task SendMessage(string str) {
            var buffer = Encoding.UTF8.GetBytes(str);
            try {
                await stream.WriteAsync(buffer, 0, str.Length);
            }
            catch {
                status_message = "Error: No established connection";
            }
        }
        
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName = "") {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
