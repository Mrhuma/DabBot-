using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Newtonsoft.Json;

namespace DabBot_
{
    class Program
    {
        private DiscordSocketClient _client;
        private List<Phrase> phrases = new List<Phrase>();
        private string path;

        public static Task Main(string[] args) => new Program().MainAsync();

        public async Task MainAsync()
        {
            _client = new DiscordSocketClient();

            _client.Log += Log;

#if DEBUG
            string token = File.ReadAllText(@"..\token.txt");
            path = @"..\phrases.json";
#else
            string token = File.ReadAllText("token.txt");
            path = "phrases.json";
#endif

            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            _client.Ready += Ready;

            //Block this task until the program is closed.
            await Task.Delay(-1);
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        private Task Ready()
        {
            //Read from the existing file if it exists
            if (File.Exists(path))
            {
                phrases = DeserializePhrases();
            }
            else
            {
                //If the file doesn't exist, create and populate with some data
                File.Create(path).Close();
                phrases.Add(new Phrase("dab", 1));
                SerializePhrases(phrases);
            }

            foreach(Phrase phrase in phrases)
            {
                Console.WriteLine("Listening to " + phrase.text + " with " + phrase.mediaCount + " images.");
            }
                return Task.CompletedTask;
        }

        //Write phrases to json file
        private void SerializePhrases(List<Phrase> phrases)
        {
            File.WriteAllText(path, JsonConvert.SerializeObject(phrases, Formatting.Indented, new Newtonsoft.Json.Converters.StringEnumConverter()));
        }

        //Read phrases from json file
        private List<Phrase> DeserializePhrases()
        {
            return JsonConvert.DeserializeObject<List<Phrase>>(File.ReadAllText(path), new Newtonsoft.Json.Converters.StringEnumConverter());
        }
    }
}
