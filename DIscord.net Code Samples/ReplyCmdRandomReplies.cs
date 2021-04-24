using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Discord.net_Code_Samples
{
    public class ReplyCmdRandomReplies : ModuleBase<SocketCommandContext> //Don't forget to set it to public
    {
        private List<string> url = new List<string>()//Store your messages there
        {
            "You",
            "Can",
            "Write",
            "Literally anything there",
            "Urls included",
            "https://www.youtube.com/watch?v=dQw4w9WgXcQ",
            "Just don't forget on commas :)"
        };

        [Command("pingRandom")]
        [Alias("ping-random", "random-ping")]
        public async Task ReplyRandomAsync()
        {
            var random = new Random();//Makes new random variable
            int rnd = random.Next(0, url.Count);//Gets the random number from 0 to the length of the list
            await ReplyAsync(url[rnd]);
            //or alternatively
            //await Context.Channel.SendMessageAsync(url[rnd]);
        }
    }
}