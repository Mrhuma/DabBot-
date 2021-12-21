using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
            //Create client
            _client = new DiscordSocketClient();
#if DEBUG
            //Fetch token and the phrases file path
            string token = File.ReadAllText(@"..\token.txt");
            path = @"..\phrases.json";
#else
            string token = File.ReadAllText("token.txt");
            path = "phrases.json";
#endif
            //Login and start the bot
            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            //Listen to event
            _client.Ready += Ready;
            _client.Log += Log;
            _client.MessageReceived += MessageReceived;

            //Block this task until the program is closed.
            await Task.Delay(-1);
        }

        private Task MessageReceived(SocketMessage msg)
        {
            //Check the message for any known phrases
            foreach(Phrase phrase in phrases)
            {
                //If a match is found
                if(phrase.regex.IsMatch(msg.CleanContent))
                {
                    //TODO
                    Console.WriteLine(phrase.folderName);
                }
            }
            
            return Task.CompletedTask;
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
                //If the file doesn't exist, create and populate with some sample data
                File.Create(path).Close();
                phrases.Add(new Phrase(new Regex("dab", RegexOptions.IgnoreCase), 1, "dab"));
                phrases.Add(new Phrase(new Regex("she+sh", RegexOptions.IgnoreCase), 1, "sheesh"));
                SerializePhrases(phrases);
            }

            //Displays each phrase and how many images are stored for the phrase
            foreach(Phrase phrase in phrases)
            {
                Console.WriteLine("Listening to " + phrase.regex + " with " + phrase.mediaCount + " images.");
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
