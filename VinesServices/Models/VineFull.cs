namespace VinesServices.Models
{
    using HtmlAgilityPack;

    public class VineFull : Vine
    {
        public string PreviousVineUrl { get; set; }

        public string NextVineUrl { get; set; }

        public string VineVideoUrl { get; set; }

        public string AddedBefore { get; set; }

        public static VineFull Parse(string html)
        {
            var vine = new VineFull();
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);
            var parrentDiv = htmlDocument.DocumentNode.SelectSingleNode("div");
            vine.Title = parrentDiv.SelectSingleNode("h1").InnerText;
            vine.PreviousVineUrl = htmlDocument.GetElementbyId("prev").Attributes["href"].Value.Replace("./", "http://vinescope.com/");
            vine.NextVineUrl = htmlDocument.GetElementbyId("next").Attributes["href"].Value.Replace("./", "http://vinescope.com/");
            vine.VinePosterUrl = parrentDiv.SelectSingleNode("div/div/div/video").Attributes["poster"].Value;
            vine.VineVideoUrl = parrentDiv.SelectSingleNode("div/div/div/video/source").Attributes["src"].Value;
            var vineDetails = parrentDiv.SelectNodes("div/div/span");
            vine.Author = vineDetails[0].InnerText.Replace("Vine By: ", string.Empty);
            vine.AddedBefore = vineDetails[1].InnerText.Replace("Added ", string.Empty).Replace(" ago ", string.Empty);

            return vine;
        }
    }
}