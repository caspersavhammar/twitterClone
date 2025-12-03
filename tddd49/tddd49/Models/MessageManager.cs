

namespace tddd49.Models {
    public class MessageManager {
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

