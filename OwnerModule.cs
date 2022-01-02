using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;

namespace DabBot_
{
    public class CommandModule : ModuleBase<SocketCommandContext>
    {
        [RequireOwner]
        [Command("update")]
        [Summary("Updates the phrases from the phrases.txt file")]
        public async Task UpdateAsync()
        {
            Program.phrases = JsonHelper.DeserializePhrases(Program.pathPrefix);
            string msg = "";

            foreach (Phrase phrase in Program.phrases)
            {
                msg += "Listening to " + phrase.regex + " with " + phrase.links.Count + " links." + Environment.NewLine;
            }

            await ReplyAsync(msg);
        }

        [Command("ping")]
        [Summary("Ping")]
        public Task HelloAsync(string msg = "pong") => ReplyAsync(msg);
    }
}
