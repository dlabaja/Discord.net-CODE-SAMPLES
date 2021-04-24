using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Discord.net_Code_Samples
{
    public class ReplyCmd : ModuleBase<SocketCommandContext> //Don't forget to set it to public
    {
        [Command("ping")]
        [Alias("ping-pls")]
        public async Task ReplyCommandAsync()
        {
            await ReplyAsync("Pong");
            //or alternatively
            //await Context.Channel.SendMessageAsync("pong");
        }
    }
}