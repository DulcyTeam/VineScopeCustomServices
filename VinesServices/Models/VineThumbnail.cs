namespace VinesServices.Models
{
    using System.Runtime.Serialization;
    using HtmlAgilityPack;

    [DataContract(Name = "vine")]
    public class VineThumbnail
    {
        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "url")]
        public string Url { get; set; }

        [DataMember(Name = "posterUrl")]
        public string PosterUrl { get; set; }

        [DataMember(Name = "author")]
        public string Author { get; set; }

        public static VineThumbnail Parse(HtmlNode node)
        {
            var vine = new VineThumbnail();

            vine.Url = node.SelectSingleNode("a").Attributes["href"].Value;
            vine.Title = node.SelectSingleNode("a/header/h3").InnerText;
            vine.PosterUrl = node.SelectSingleNode("a/div/img").Attributes["src"].Value;
            vine.Author = node.SelectSingleNode("a/footer/p").InnerText;

            return vine;
        }
    }
}