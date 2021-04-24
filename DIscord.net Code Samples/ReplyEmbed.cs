using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Discord.net_Code_Samples
{
    public class ReplyCmdEmbed : ModuleBase<SocketCommandContext> //Don't forget to set it to public
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

        [Command("pingEmbed")]
        [Alias("ping-embed", "embed-ping")]
        public async Task ReplyEmbedAsync()
        {
            var random = new Random();//Makes new random variable
            int rnd = random.Next(0, url.Count);//Gets the random number from 0 to the length of the list

            var emb = new EmbedBuilder(); //makes new embed
            emb.WithTitle("**This is embed. See?**"); //add title
            emb.WithImageUrl(url[rnd]); //add image from list
            emb.WithColor(Color.Red); //Color of an embed
            emb.WithFooter(footer => //footer of the embed
            {
                footer
                .WithIconUrl("https://cdn.discordapp.com/emojis/778284745448357888.png?v=1")
                .WithText("Footer is epic, right?");
            });
            await Context.Channel.SendMessageAsync(embed: emb.Build()); //sends embed to current channel (Or just use ReplyAsync() instead)
        }
    }
}