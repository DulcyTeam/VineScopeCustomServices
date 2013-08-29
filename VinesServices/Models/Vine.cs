namespace VinesServices.Models
{
    using HtmlAgilityPack;

    public class Vine
    {
        public string Title { get; set; }

        public string VineUrl { get; set; }

        public string VinePosterUrl { get; set; }

        public string Author { get; set; }

        public static Vine Parse(string html)
        {
            var vine = new Vine();

            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);
            var articleNode = htmlDocument.DocumentNode.SelectSingleNode("article");
            vine.VineUrl = articleNode.SelectSingleNode("a").Attributes["href"].Value;
            vine.Title = articleNode.SelectSingleNode("a/header/h3").InnerText;
            vine.VinePosterUrl = articleNode.SelectSingleNode("a/div/img").Attributes["src"].Value;
            vine.Author = articleNode.SelectSingleNode("a/footer/p").InnerText;

            return vine;
        }
    }
}