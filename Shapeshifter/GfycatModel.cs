using System;
using System.Collections.Generic;
using System.Text;

namespace Shapeshifter
{
    class GfycatModel
    {

        public Gfyitem gfyItem { get; set; }
        
        public class Gfyitem
        {
            public string[] tags { get; set; }
            public object[] languageCategories { get; set; }
            public object[] domainWhitelist { get; set; }
            public object[] geoWhitelist { get; set; }
            public int published { get; set; }
            public string nsfw { get; set; }
            public int gatekeeper { get; set; }
            public string mp4Url { get; set; }
            public string gifUrl { get; set; }
            public string webmUrl { get; set; }
            public string webpUrl { get; set; }
            public string mobileUrl { get; set; }
            public string mobilePosterUrl { get; set; }
            public string extraLemmas { get; set; }
            public string thumb100PosterUrl { get; set; }
            public string miniUrl { get; set; }
            public string gif100px { get; set; }
            public string miniPosterUrl { get; set; }
            public string max5mbGif { get; set; }
            public string title { get; set; }
            public string max2mbGif { get; set; }
            public string max1mbGif { get; set; }
            public string posterUrl { get; set; }
            public string languageText { get; set; }
            public int views { get; set; }
            public string userName { get; set; }
            public string description { get; set; }
            public bool hasTransparency { get; set; }
            public bool hasAudio { get; set; }
            public string likes { get; set; }
            public string dislikes { get; set; }
            public string gfyNumber { get; set; }
            public string userDisplayName { get; set; }
            public string userProfileImageUrl { get; set; }
            public string gfyId { get; set; }
            public string gfyName { get; set; }
            public string avgColor { get; set; }
            public string rating { get; set; }
            public string gfySlug { get; set; }
            public int width { get; set; }
            public int height { get; set; }
            public float frameRate { get; set; }
            public float numFrames { get; set; }
            public int mp4Size { get; set; }
            public int webmSize { get; set; }
            public int createDate { get; set; }
            public string md5 { get; set; }
            public int source { get; set; }
            public Content_Urls content_urls { get; set; }
            public Userdata userData { get; set; }
        }

        public class Content_Urls
        {
            public Max2mbgif max2mbGif { get; set; }
            public Webp webp { get; set; }
            public Max1mbgif max1mbGif { get; set; }
            public _100Pxgif _100pxGif { get; set; }
            public Mobileposter mobilePoster { get; set; }
            public Mp4 mp4 { get; set; }
            public Webm webm { get; set; }
            public Max5mbgif max5mbGif { get; set; }
            public Largegif largeGif { get; set; }
            public Mobile mobile { get; set; }
        }

        public class Max2mbgif
        {
            public string url { get; set; }
            public int size { get; set; }
            public int height { get; set; }
            public int width { get; set; }
        }

        public class Webp
        {
            public string url { get; set; }
            public int size { get; set; }
            public int height { get; set; }
            public int width { get; set; }
        }

        public class Max1mbgif
        {
            public string url { get; set; }
            public int size { get; set; }
            public int height { get; set; }
            public int width { get; set; }
        }

        public class _100Pxgif
        {
            public string url { get; set; }
            public int size { get; set; }
            public int height { get; set; }
            public int width { get; set; }
        }

        public class Mobileposter
        {
            public string url { get; set; }
            public int size { get; set; }
            public int height { get; set; }
            public int width { get; set; }
        }

        public class Mp4
        {
            public string url { get; set; }
            public int size { get; set; }
            public int height { get; set; }
            public int width { get; set; }
        }

        public class Webm
        {
            public string url { get; set; }
            public int size { get; set; }
            public int height { get; set; }
            public int width { get; set; }
        }

        public class Max5mbgif
        {
            public string url { get; set; }
            public int size { get; set; }
            public int height { get; set; }
            public int width { get; set; }
        }

        public class Largegif
        {
            public string url { get; set; }
            public int size { get; set; }
            public int height { get; set; }
            public int width { get; set; }
        }

        public class Mobile
        {
            public string url { get; set; }
            public int size { get; set; }
            public int height { get; set; }
            public int width { get; set; }
        }

        public class Userdata
        {
            public string name { get; set; }
            public string profileImageUrl { get; set; }
            public string url { get; set; }
            public string username { get; set; }
            public int followers { get; set; }
            public int subscription { get; set; }
            public int following { get; set; }
            public string profileUrl { get; set; }
            public int views { get; set; }
            public bool verified { get; set; }
        }

    }
}
