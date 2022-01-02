﻿using System;
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
        private string pathPrefix = "";

        public static Task Main(string[] args) => new Program().MainAsync();

        public async Task MainAsync()
        {
            //Create client
            _client = new DiscordSocketClient();
#if DEBUG
            //Fetch token and the phrases file path
            string token = File.ReadAllText(@"..\token.txt");
            pathPrefix = @"..\";
#else
            string token = File.ReadAllText("token.txt");
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
            //Ensure that no bot can trigger a response
            if (!msg.Author.IsBot)
            {
                //Check the message for any known phrases
                foreach (Phrase phrase in phrases)
                {
                    //If a match is found
                    if (phrase.regex.IsMatch(msg.CleanContent))
                    {
                        //Randomly select a number corresponding to a file in the list
                        int random = new Random().Next(1, phrase.links.Count);

                        Embed embed = new EmbedBuilder
                        {
                            ImageUrl = phrase.links[random]
                        }.Build();

                        msg.Channel.SendMessageAsync("", embed: embed);
                    }
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
            if (File.Exists(pathPrefix + "phrases.json"))
            {
                phrases = DeserializePhrases();
            }
            else
            {
                //If the file doesn't exist, create and populate with some sample data
                File.Create(pathPrefix + "phrases.json").Close();
                List<string> dabLinks = new List<string>()
                {
                    "http://mrhumagames.com/DabBot/dab/unicorn.png",
                    "http://mrhumagames.com/DabBot/dab/panda.png",
                    "http://mrhumagames.com/DabBot/dab/pepe.png",
                    "http://mrhumagames.com/DabBot/dab/roblox.png",
                    "http://mrhumagames.com/DabBot/dab/cat.png",
                    "http://mrhumagames.com/DabBot/dab/dog.png",

                };
                phrases.Add(new Phrase(new Regex("dab", RegexOptions.IgnoreCase), dabLinks));

                List<string> sheeshLinks = new List<string>()
                {
                    "http://mrhumagames.com/DabBot/sheesh/1.png",
                    "http://mrhumagames.com/DabBot/sheesh/2.png",
                    "http://mrhumagames.com/DabBot/sheesh/3.png",
                    "http://mrhumagames.com/DabBot/sheesh/4.png",
                    "http://mrhumagames.com/DabBot/sheesh/5.png",

                };
                phrases.Add(new Phrase(new Regex("shee+sh", RegexOptions.IgnoreCase), sheeshLinks));
                SerializePhrases(phrases);
            }

            //Displays each phrase and how many images are stored for the phrase
            foreach(Phrase phrase in phrases)
            {
                Console.WriteLine("Listening to " + phrase.regex + " with " + phrase.links.Count + " links.");
            }
                return Task.CompletedTask;
        }

        //Write phrases to json file
        private void SerializePhrases(List<Phrase> phrases)
        {
            File.WriteAllText(pathPrefix + "phrases.json", JsonConvert.SerializeObject(phrases, Formatting.Indented, new Newtonsoft.Json.Converters.StringEnumConverter()));
        }

        //Read phrases from json file
        private List<Phrase> DeserializePhrases()
        {
            return JsonConvert.DeserializeObject<List<Phrase>>(File.ReadAllText(pathPrefix + "phrases.json"), new Newtonsoft.Json.Converters.StringEnumConverter());
        }
    }
}
