﻿namespace VinesServices.Models
{
    using System.Linq;
    using System.Runtime.Serialization;
    using HtmlAgilityPack;

    [DataContract(Name = "vine")]
    public class Vine : VineThumbnail
    {
        [DataMember(Name = "previousVineUrl")]
        public string PreviousVineUrl { get; set; }

        [DataMember(Name = "nextVineUrl")]
        public string NextVineUrl { get; set; }

        [DataMember(Name = "videoUrl")]
        public string VideoUrl { get; set; }

        [DataMember(Name = "addedBefore")]
        public string AddedBefore { get; set; }

        public static string GetUrl(VineThumbnail vine)
        {
            string vineUrl = vine.PosterUrl.Substring(vine.PosterUrl.LastIndexOf("/") + 1).Replace("jpg", "html");
            return vineUrl;
        }

        public static Vine Parse(HtmlNode node)
        {
            var vine = new Vine();

            vine.Title = node.SelectSingleNode("h1").InnerText;
            var neighbourVideos = node.SelectNodes("div/a");
            vine.PreviousVineUrl = neighbourVideos
                                                  .Where(n => n.Attributes["id"].Value == "prev")
                                                  .FirstOrDefault()
                                                  .Attributes["href"].Value.Replace("./", string.Empty);
            vine.NextVineUrl = neighbourVideos
                                              .Where(n => n.Attributes["id"].Value == "next")
                                              .FirstOrDefault()
                                              .Attributes["href"].Value.Replace("./", string.Empty);
            vine.PosterUrl = node.SelectSingleNode("div/div/video").Attributes["poster"].Value;
            vine.VideoUrl = node.SelectSingleNode("div/div/video/source").Attributes["src"].Value;
            var vineAuthorName = node.SelectSingleNode("div/div/div/p/strong/span").InnerText;
            vine.Author = vineAuthorName;
            var addedBefore = node.SelectSingleNode("div/div/span").InnerText;
            vine.AddedBefore = addedBefore.Replace("Added ", string.Empty).Replace(" ago ", string.Empty);

            return vine;
        }

        public VineThumbnail VineThumbnail
        {
            get
            {
                return new VineThumbnail()
                {
                    Author = this.Author,
                    PosterUrl = this.PosterUrl,
                    Title = this.Title,
                    Url = this.Url,
                };
            }
        }
    }
}