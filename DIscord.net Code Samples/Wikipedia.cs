using Discord.Commands;
using System.Threading.Tasks;
using Genbox.Wikipedia.Enums;
using Genbox.Wikipedia.Objects;
using Genbox.Wikipedia;
using System;
using Discord;
using System.Net;
using System.Text.RegularExpressions;
using Discord.WebSocket;
using System.Linq;

namespace MilošBot.Commands
{
    public class Wikipedia : ModuleBase
    {
        //You can use whatever language you want, I used CZ, SK and EN
        private static Language[] _langs = new Language[] { Language.Czech, Language.Slovak, Language.English };

        private WikipediaClient client = new WikipediaClient();

        [Command("wiki")]
        public async Task WikiSearch([Remainder] string text = null)
        {
            if (text == null)
            {
                await ReplyAsync("What should I look for?");
                return;
            }

            using (Context.Channel.EnterTypingState())
            {
                foreach (var lang in _langs)
                {
                    var hledani = await vyhledavani(lang, text);
                    if (hledani != null)
                    {
                        await ReplyAsync(embed: hledani.Build());
                        return;
                    }
                }
                await ReplyAsync("Bohužel jsem nic nenašel");
            }
        }

        private async Task<EmbedBuilder> vyhledavani(Language jazyk, string text)
        {
            Console.WriteLine("poprve");
            client.UseTls = true;
            client.Limit = 1;
            client.What = What.Text;
            client.Language = jazyk;
            QueryResult results = await client.SearchAsync(text);
            foreach (var vysledek in results.Search)
            {
                if (vysledek.Url.ToString() == "https://cs.wikipedia.org/wiki/" || vysledek.Url.ToString() == "https://sk.wikipedia.org/wiki/" || vysledek.Url.ToString() == "https://en.wikipedia.org/wiki/")
                    return null;
                //return vysledek.Url.ToString();
                var ovcak = new EmbedBuilder();
                ovcak.WithTitle(vysledek.Title);
                ovcak.WithUrl(vysledek.Url.ToString().Replace(" ", "_"));
                ovcak.WithDescription("**" + HtmlToPlainText(vysledek.Snippet) + "**");
                ovcak.WithFooter(footer =>
                {
                    footer
                    .WithIconUrl("https://cdn.discordapp.com/emojis/778284745448357888.png?v=1")
                    .WithText("Wikipedie kterou ukradl MilošBot");
                });
                return ovcak;
            }
            return null;
        }

        private static string HtmlToPlainText(string html)
        {
            const string tagWhiteSpace = @"(>|$)(\W|\n|\r)+<";//matches one or more (white space or line breaks) between '>' and '<'
            const string stripFormatting = @"<[^>]*(>|$)";//match any character between '<' and '>', even when end tag is missing
            const string lineBreak = @"<(br|BR)\s{0,1}\/{0,1}>";//matches: <br>,<br/>,<br />,<BR>,<BR/>,<BR />
            var lineBreakRegex = new Regex(lineBreak, RegexOptions.Multiline);
            var stripFormattingRegex = new Regex(stripFormatting, RegexOptions.Multiline);
            var tagWhiteSpaceRegex = new Regex(tagWhiteSpace, RegexOptions.Multiline);

            var text = html;
            //Decode html specific characters
            text = System.Net.WebUtility.HtmlDecode(text);
            //Remove tag whitespace/line breaks
            text = tagWhiteSpaceRegex.Replace(text, "><");
            //Replace <br /> with line breaks
            text = lineBreakRegex.Replace(text, Environment.NewLine);
            //Strip formatting
            text = stripFormattingRegex.Replace(text, string.Empty);

            return text;
        }
    }
}