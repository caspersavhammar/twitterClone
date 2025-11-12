using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Demo.Model;
using Demo.View;
using Demo.View.Command;

namespace Demo.ViewModel
{
    internal class MainWindowViewModel : INotifyPropertyChanged
    {

        private NetworkManager NetworkManager { get; set; }
        private ICommand startGame;
        private string text;
        public string MyText { 
            get {

                return text; } 
            set 
            { 
                text = value;
                OnPropertyChanged("MyText");
            
            } 
        }
        

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public MainWindowViewModel(NetworkManager networkManager)
        {
            NetworkManager = networkManager;
            networkManager.PropertyChanged += myModel_PropertyChanged;
        }

        private void myModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Message")
            {
                var message = NetworkManager.Message;
                this.MyText = message;
            }
        }

        public ICommand StartGame
        {
            get
            {
                if (startGame == null)
                    startGame = new StartGameCommand(this);
                return startGame;
            }
            set
            {
                startGame = value;
            }
        }

        private bool startConnection()
        {
            return NetworkManager.startConnection();
        }

        public void startGameBoard()
        {

            if (startConnection())
            {
                GameBoard board = new GameBoard();
                board.DataContext = this;
                board.ShowDialog();
            }
            else
            {
                MessageBox.Show("Cannot start connection!");
            }
            
            
           
        }

        private ICommand enterCommand;
        public ICommand EnterCommand
        {
            get {
                if (enterCommand == null)
                {
                    return new Command.KeyEnterCommand(this);
                }
                else {
                    return enterCommand;

                }
            }
            set { enterCommand = value; }
        }

        public void sendMessage()
        {
            NetworkManager.sendChar(MyText);
        }

        public void showGameBoard()
        {
            GameBoard gameBoard = new GameBoard();
            gameBoard.DataContext = this;
            gameBoard.ShowDialog();
        }


    }
}
