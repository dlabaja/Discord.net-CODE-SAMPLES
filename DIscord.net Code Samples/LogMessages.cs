using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Discord.net_Code_Samples
{
    public class LogCmd : ModuleBase<SocketCommandContext> //Don't forget to set it to public
    {
        [Command("ping")]
        [Alias("ping-pls")]
        public async Task LogCommandAsync()
        {
            await ReplyAsync("Pong");
            //or alternatively
            //await Context.Channel.SendMessageAsync("pong");

            ITextChannel channel = Context.Client.GetChannel(00000000000/*Replace with channel id*/) as ITextChannel;
            await channel.SendMessageAsync(Context.User + " used command Pong in channel " + Context.Channel.Name); //Sends message to specified channel
            //Context.User = user who used the command
            //Context.Channel.Name = channel where it was used
        }
    }
}