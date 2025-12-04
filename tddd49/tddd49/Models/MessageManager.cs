using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.IO;
using System.Linq;

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

        public static message_template MessageToTemplate(string message) {
            string[] contents = message.Split("DEL");
            return new message_template(contents[0], contents[1]);
        }

        public static message_template MessageToTemplate(string message, string from) {
            return new message_template(message, from);
        }

        public static void SaveConversation(ObservableCollection<message_template> conversation, string to)
        {

            List<Tuple<string, ObservableCollection<message_template>>> file_contents = JsonSerializer.Deserialize<List<Tuple<string, ObservableCollection<message_template>>>>(File.ReadAllText("db/db.json"));

            file_contents.Add(new Tuple<string, ObservableCollection<message_template>>(to, conversation));

            File.WriteAllText("db/db.json", JsonSerializer.Serialize(file_contents));
        }
    }
};

