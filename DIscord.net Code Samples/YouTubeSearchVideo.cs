using Discord;
using Discord.Commands;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using System;
using System.Threading.Tasks;

namespace MilošBot.Commands
{
    public class Najdi : ModuleBase<SocketCommandContext>
    {
        //AIzaSyBnWN0zHpEBIEyy10_mpQU2i7QWYXZPZGA

        private string videoid;

        [Command("yt-search")]
        public async Task Run([Remainder] string name = null)
        {
            if (name == null)
            {
                await ReplyAsync("What should I search for?");
                return;
            }

            //register service
            var yt = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = "YourApiKey",
                ApplicationName = "YourGoogleCloudProjectName"
            });

            using (Context.Channel.EnterTypingState())
            {
                //query the youtube
                var searchListRequest = yt.Search.List("snippet,id");
                searchListRequest.Q = name;
                searchListRequest.MaxResults = 1;
                searchListRequest.Type = "video";
                searchListRequest.EventType = SearchResource.ListRequest.EventTypeEnum.None;
                var searchListResponse = await searchListRequest.ExecuteAsync();

                //if no items are found
                if (searchListResponse.Items.Count == 0)
                {
                    await ReplyAsync("Nothing found");
                    return;
                }
                //gets an id from result
                foreach (var video in searchListResponse.Items)
                {
                    videoid = video.Id.VideoId;
                }
            }
            await ReplyAsync("Yay! I found the video! https://www.youtube.com/watch?v=" + videoid);
        }
    }
}