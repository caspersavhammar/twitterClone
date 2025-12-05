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

        public static ObservableCollection<Tuple<string, ObservableCollection<MessageManager.message_template>>> get_db()
        {
            return new ObservableCollection<Tuple<string, ObservableCollection<MessageManager.message_template>>>(
                JsonSerializer
                    .Deserialize<List<Tuple<string, ObservableCollection<MessageManager.message_template>>>>(
                        File.ReadAllText("db/db.json")));
        }

        public static void SaveConversation(ObservableCollection<message_template> conversation, string? to) {
            if (to == null) {
                return;
            }
            List<Tuple<string, ObservableCollection<message_template>>> file_contents = JsonSerializer.Deserialize<List<Tuple<string, ObservableCollection<message_template>>>>(File.ReadAllText("db/db.json"));
            file_contents.Add(new Tuple<string, ObservableCollection<message_template>>(to, conversation));
            File.WriteAllText("db/db.json", JsonSerializer.Serialize(file_contents));
        }

        public static ObservableCollection<Tuple<string, ObservableCollection<message_template>>> SearchHistory(
        ObservableCollection<Tuple<string, ObservableCollection<message_template>>> history, string query) {
            if (query == "") {
                return history;
            }
            
            ObservableCollection<Tuple<string, ObservableCollection<message_template>>> result_collection =
            new ObservableCollection<Tuple<string, ObservableCollection<message_template>>>();

            IEnumerable<Tuple<string, ObservableCollection<message_template>>> search_query =
                from item in history
                where item.Item1.Contains(query)
                select item;
            
            foreach (var i in search_query) {
                result_collection.Add(i);
            }
            
            return result_collection;
        }
    }
};

