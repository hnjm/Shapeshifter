# Telegram photo/video bot for .NET

A .Net Core application used to send photos and videos via telegram. Booru exstensibility is there as well with the /search function. There is a lot of base functunality here that can be fleshed out in the future if I feel like it but anyone can take this base and apply it to their own project pretty easily. Grabs photos from a /command and a subreddit either with a post number (via hot) or without so it's random. Has the ability to grab gfycat videos as well from subreddits and the ability to convert videos on the fly that do not have .mp4 extension (since telegram can only send gif's and .mp4's). You can add admin groups and users to the member list so only you can use the bot or other people can as well, or you can remove that feature entirely if you wish.

## Basic Usage

If you want to use this program yourself, you will need a couple things.

- [x] Add your own App.config to the project.
- [x] Add keys and values from the following list to your App.config:

```csharp
telegramAdmins = memberIds
telegramKey = yourSuperSecretBotKey
redditAppId = yourRedditAppId
redditSecret = youSuperSecretRedditSecret
redditRefresh = redditRefreshToken
commandList = /commands,/seperated,/by,/a,/comma
subreddits = thesubreddits,that,you,want,seperated,bya,comma,inthe,same,order
specialCommand = /commandForVideos
specialSubreddit = yourVideoSubbredit
helpString = your help or start string, use &#xA; for new lines
```

- [x] Build/Publish your application for your desired platform

If there are any questions, bugs, or feature requests please open an issue on gitlab! :dog:
