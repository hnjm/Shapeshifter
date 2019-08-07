using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using Reddit;
using Booru.Net;
using System.Net;
using Xabe.FFmpeg;
using System.IO;
using System.Configuration;
using System.Threading;

namespace Shapeshifter
{
    class ImageMaster
    {
        private bool sent;
        private static ApiCalls work = new ApiCalls();
        private readonly TelegramBotClient Bot = new TelegramBotClient(ConfigurationManager.AppSettings["telegramKey"]);
        private readonly RedditAPI reddit = new RedditAPI(appId: ConfigurationManager.AppSettings["redditAppId"], appSecret: ConfigurationManager.AppSettings["redditSecret"], refreshToken: ConfigurationManager.AppSettings["redditRefresh"]);
        BooruClient BooruClient = new BooruClient();
        private Random random = new Random();
        private readonly List<long> admins = new List<long>(ConfigurationManager.AppSettings["telegramAdmins"].Split(',').Select(Int64.Parse).ToList());
        private readonly List<string> commandList = new List<string>(ConfigurationManager.AppSettings["commandList"].Split(new char[] { ',' }));
        private readonly List<string> subreddits = new List<string>(ConfigurationManager.AppSettings["subreddits"].Split(new char[] { ',' }));
        private readonly List<string> stickers = new List<string> {
            "CAADBAADCgEAAoj_wBXErjNOBNPjNgI",
            "CAADBAAD5wIAAi2qrAghi_faoylKlwI",
            "CAADBAADoQIAAhpJiAiq9HRcsC1JMAI",
            "CAADBAADFAEAAi2qrAgV-dj24oUU2wI",
            "CAADBAAD9wADiP_AFTt9Htl76XOHAg",
            "CAADAQADsAADXWaGDWPyTIw7TArcAg",
            "CAADBAADlQADiP_AFeaD48cJ3umGAg"
        };

        public ImageMaster()
        {
            var me = Bot.GetMeAsync().Result;
            Console.Title = me.Username;
            Bot.OnMessage += BotOnMessageReceived;
            Bot.OnReceiveError += BotOnReceiveError;

            Bot.StartReceiving(Array.Empty<UpdateType>());
            Console.WriteLine($"Start listening for @{me.Username}");
            Thread.Sleep(Timeout.Infinite);
            Bot.StopReceiving();
            
        }

        private void BotOnReceiveError(object sender, ReceiveErrorEventArgs receiveErrorEventArgs)
        {
            Console.WriteLine("Received error: {0} — {1}",
                receiveErrorEventArgs.ApiRequestException.ErrorCode,
                receiveErrorEventArgs.ApiRequestException.Message);
        }

        private async void BotOnMessageReceived(object sender, MessageEventArgs messageEventArgs)
        {
            var message = messageEventArgs.Message;

            if (message == null || message.Type != MessageType.Text ) return;

            var telegramMessage = message.Text.Split(' ').First();
            var vidOrPicNumber = message.Text.Split(' ').Last();
            // Start commands
            RestrictedMember(message);
            if (telegramMessage == "/search")
            {
                var searchString = message.Text.Split(' ').Last();
                var posts = await BooruClient.GetSafebooruImagesAsync(searchString);
                if (posts != null)
                {
                    int randomPostNumber = random.Next(posts.Count);
                    if (posts[randomPostNumber].Rating.ToString() == "Safe")
                    {
                        await Bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
                        await Bot.SendPhotoAsync(message.Chat.Id, posts[randomPostNumber].ImageUrl);
                    }
                    else
                    {
                        await Bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
                        await Task.Delay(500);
                        await Bot.SendTextMessageAsync(message.Chat.Id, "Sowwy, that image is nyot safe (・`ω´・)");
                    }
                }
            }
            else if (telegramMessage == "/help" || telegramMessage == "/start")
            {
                await Bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
                await Task.Delay(500);
                await Bot.SendTextMessageAsync(message.Chat.Id, HelpOrStart());
            }
            else if (telegramMessage == "/roll" || telegramMessage == "/dice")
            {
                await Bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
                await Task.Delay(500);
                await Bot.SendTextMessageAsync(message.Chat.Id, random.Next(101).ToString());
            }
            else if (telegramMessage == ConfigurationManager.AppSettings["specialCommand"] && message.Chat.Id == 211285870)
            {
                try
                {
                    // Block the thread until this task finishes, kind of unintuitive but this works
                    if (int.TryParse(vidOrPicNumber, out int n))
                    {
                        var myTask = Task.Run(() => DownloadFile(GetRedditVideo(Convert.ToInt32(vidOrPicNumber))));
                        myTask.Wait();
                        var fileNames = myTask.Result;
                        await Bot.SendChatActionAsync(message.Chat.Id, ChatAction.UploadVideo);
                        if (!fileNames[1].Contains("gfycat"))
                        {
                            using (var fileStream = new FileStream(fileNames[1], FileMode.Open, FileAccess.Read, FileShare.Read))
                            {
                                await Bot.SendVideoAsync(message.Chat.Id, fileStream);
                            }
                            await DeleteFile(fileNames);
                            fileNames.Clear();
                        }
                        else
                        {
                            await Bot.SendVideoAsync(message.Chat.Id, fileNames[1]);
                            fileNames.Clear();
                        }
                    }
                    else
                    {
                        var myTask = Task.Run(() => DownloadFile(GetRedditVideo()));
                        myTask.Wait();
                        var fileNames = myTask.Result;
                        await Bot.SendChatActionAsync(message.Chat.Id, ChatAction.UploadVideo);
                        if (!fileNames[1].Contains("gfycat"))
                        {
                            using (var fileStream = new FileStream(fileNames[1], FileMode.Open, FileAccess.Read, FileShare.Read))
                            {
                                await Bot.SendVideoAsync(message.Chat.Id, fileStream);
                            }
                            await DeleteFile(fileNames);
                            fileNames.Clear();
                        }
                        else
                        {
                            await Bot.SendVideoAsync(message.Chat.Id, fileNames[1]);
                            fileNames.Clear();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"There was an error with videos: {ex}");
                }

            }
            else
            {
                foreach (var command in commandList)
                {
                    if (telegramMessage == command)
                    {
                        
                        await Bot.SendChatActionAsync(message.Chat.Id, ChatAction.UploadPhoto);
                        if (int.TryParse(vidOrPicNumber, out int n))
                        {
                            await Bot.SendPhotoAsync(message.Chat.Id, GetRedditPicture(commandList[commandList.IndexOf(command)], Convert.ToInt32(vidOrPicNumber)));
                        }
                        else
                        {
                            await Bot.SendPhotoAsync(message.Chat.Id, GetRedditPicture(commandList[commandList.IndexOf(command)]));
                        }
                        sent = true;
                        break;
                    }
                    else
                    {
                        sent = false;
                    }
                }
                if (!sent)
                {
                    await Bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
                    await Bot.SendStickerAsync(message.Chat.Id, stickers[random.Next(stickers.Count)]);
                }
            }
        }


        private string GetRedditVideo()
        {
            var subreddit = reddit.Subreddit(ConfigurationManager.AppSettings["specialSubreddit"]).About();
            var posts = subreddit.Posts.GetHot(limit: 100);
            return posts[random.Next(100)].Listing.URL;
        }

        private string GetRedditVideo(int videoNumber)
        {
            if (videoNumber <= 100 && videoNumber >= 0)
            {
                var subreddit = reddit.Subreddit(ConfigurationManager.AppSettings["specialSubreddit"]).About();
                var posts = subreddit.Posts.GetHot(limit: 100);
                return posts[videoNumber].Listing.URL;
            }
            else
            {
                return GetRedditVideo();
            }
        }

        private string GetRedditPicture(string whichSubreddit)
        {
            var subreddit = reddit.Subreddit(subreddits[commandList.IndexOf(whichSubreddit)]).About();
            var posts = subreddit.Posts.GetHot(limit: 100);
            return posts[random.Next(100)].Listing.URL;
        }

        private string GetRedditPicture(string whichSubreddit, int pictureNumber)
        {
            if (pictureNumber <= 100 && pictureNumber >= 0)
            {
                var subreddit = reddit.Subreddit(subreddits[commandList.IndexOf(whichSubreddit)]).About();
                var posts = subreddit.Posts.GetHot(limit: 100);
                return posts[pictureNumber].Listing.URL;
            }
            else
            {
                return GetRedditPicture(whichSubreddit);
            }
        }

        private bool RestrictedMember(Telegram.Bot.Types.Message message)
        {
            if (admins.Contains(message.Chat.Id))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private string HelpOrStart()
        {
            return ConfigurationManager.AppSettings["helpString"];
        }


        private async Task<List<string>> DownloadFile(string url)
        {
            string newFileName;
            if (url.Contains("gfycat.com"))
            {
                CancellationTokenSource source = new CancellationTokenSource();
                CancellationToken token = source.Token;

                newFileName = url.Split('/')[3];
                newFileName = newFileName.Split('.')[0];
                var gfyCat_data = Task.Run(() => ApiCalls.GfyCat("https://api.gfycat.com/v1/gfycats/" + newFileName, token));
                gfyCat_data.Wait();
                return new List<string> { "null", gfyCat_data.Result.gfyItem.mobileUrl };
            }
            else if (url.Contains("redd"))
            {
                newFileName = url.Split('/')[3];
                using (var client = new WebClient())
                {
                    var myTask = Task.Run(() => client.DownloadFileAsync(new Uri(url), newFileName));
                    myTask.Wait();
                }
                var files = await ConvertFile(newFileName);
                return files;
            }
            return new List<string> { "null", "null" };
        }

        private async Task<List<string>> ConvertFile(string newFileName)
        {
            List<string> files = new List<string>();
            FFmpeg.ExecutablesPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "FFmpeg");
            await FFmpeg.GetLatestVersion();
            string outputFileName = Path.ChangeExtension(newFileName, ".mp4");
            if (File.Exists(outputFileName))
                File.Delete(outputFileName);
            await Conversion.ToMp4(newFileName, outputFileName).Start();
            await Console.Out.WriteLineAsync($"Finished conversion file [{newFileName}]");
            files.Add(newFileName);
            files.Add(outputFileName);
            return files;
        }

        private async Task DeleteFile(List<string> fileNames)
        {
            foreach (var file in fileNames)
            {
                FileInfo fi = new FileInfo(file);
                if (fi.Exists)
                    await fi.DeleteAsync();
            }
        }
    }

}
