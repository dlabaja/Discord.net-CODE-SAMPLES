using Discord;
using Discord.Commands;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using System;
using System.Threading.Tasks;

namespace MilošBot.Commands
{
    public class YTRandom : ModuleBase<SocketCommandContext>
    {
        public static char GetLetter()
        {
            string chars = "abcdefghijklmnopqrstuvwxyz";
            Random rand = new Random();
            int num = rand.Next(0, chars.Length);
            return chars[num];
        }

        private string videoid;
        private int i = 0;
        private ulong e = 1000;
        private ulong u;
        private DateTime time;

        [Command("yt-random")]
        public async Task Run()
        {
            //register service
            var yt = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = "YourApiKey",
                ApplicationName = "YourGoogleCloudProjectName"
            });

            using (Context.Channel.EnterTypingState())
            {
                //add video parameters
                var searchListRequest = yt.Search.List("snippet,id");
                searchListRequest.Q = GetLetter().ToString() + GetLetter().ToString(); //generates random two letters for a quality content hehe
                searchListRequest.MaxResults = 50;
                searchListRequest.Order = SearchResource.ListRequest.OrderEnum.ViewCount;
                searchListRequest.Type = "video";
                searchListRequest.VideoDuration = SearchResource.ListRequest.VideoDurationEnum.Short__; //short videos best videos
                searchListRequest.PublishedAfter = DateTime.Now.AddHours(-15);
                searchListRequest.EventType = SearchResource.ListRequest.EventTypeEnum.None; //no livestreams
                var searchListResponse = await searchListRequest.ExecuteAsync(); //this will return about 50 videos
                i = 0;
                e = 1000;

                //gets the video with lowest subscribers
                foreach (var video in searchListResponse.Items)
                {
                    if (i > searchListResponse.Items.Count - 10)
                    {
                        var subscriptionListRequest = yt.Channels.List("contentDetails,statistics,snippet");
                        subscriptionListRequest.Id = video.Snippet.ChannelId;
                        subscriptionListRequest.MaxResults = 1;
                        var searchListResult = subscriptionListRequest.Execute();
                        foreach (var kanal in searchListResult.Items)
                        {
                            if (kanal.Statistics.SubscriberCount < e)
                            {
                                e = (ulong)kanal.Statistics.SubscriberCount;
                                videoid = video.Id.VideoId;
                                time = (DateTime)video.Snippet.PublishedAt;
                            }
                        }
                    }
                    i++;
                }

                //find stats for the video
                var vid = yt.Videos.List("statistics");
                vid.Id = videoid;
                vid.MaxResults = 1;
                var hledej = await vid.ExecuteAsync();
                foreach (var video in hledej.Items)
                {
                    u = (ulong)video.Statistics.ViewCount;
                    break;
                }
            }
            await ReplyAsync("https://www.youtube.com/watch?v=" + videoid + "\n**Subscribers:** " + e + ", **Views:** " + u + ", **Published:** " + time);
            i = 0;
            e = 1000;
        }
    }
}