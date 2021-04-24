using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Discord.net_Code_Samples
{
    public class ForSpecificRole : ModuleBase<SocketCommandContext> //Don't forget to set it to public
    {
        [Command("role")]
        public async Task Ping([Remainder] string user = null)
        {
            Emote emoji = Emote.Parse(/*Emote with id, like this:*/" <:testemote:814392134878101524>");
            var UserContext = Context.User as SocketGuildUser; //user who triggered the command

            //Finding the role in the current server
            var role = Context.Guild.Roles.FirstOrDefault(x => x.Name == "RoleName");
            var role2 = Context.Guild.Roles.FirstOrDefault(x => x.Name == "AnotherRoleName");

            using (Context.Channel.EnterTypingState()) //Triggers the User is typing... message
            {
                if (UserContext.Roles.Contains(role) || UserContext.Roles.Contains(role2)) //Checking if user has the role
                {
                    await ReplyAsync("Yeah, you can do this" + emoji);
                }
                else
                {
                    await ReplyAsync("You don't have high enough role");
                }
            }
        }
    }
}