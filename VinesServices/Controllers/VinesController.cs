namespace VinesServices.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Web.Http;
    using AttributeRouting.Web.Http;
    using HtmlAgilityPack;
    using VinesServices.Models;

    public class VinesController : ApiController
    {
        private const string vineScopeUrl = "http://vinescope.com/";

        // GET api/vines
        //public IEnumerable<VineThumbnail> Get()
        //{
        //    string htmlString = VineScopeHtmlRequester();

        //    var htmlDocument = new HtmlDocument();
        //    htmlDocument.LoadHtml(htmlString);

        //    var indexVideoNode = htmlDocument.GetElementbyId("container").SelectSingleNode("div");
        //    VineThumbnail indexMainVine = Vine.Parse(indexVideoNode);
        //    indexMainVine.Url = vineScopeUrl;
        //    var vines = new List<VineThumbnail>();
        //    vines.Add(indexMainVine);

        //    var featuredVinesNodes = htmlDocument.GetElementbyId("container")
        //                                         .SelectNodes("div/div/div")
        //                                         .Where(n => n.Attributes["class"] != null && n.Attributes["class"].Value == "thumb-list")
        //                                         .FirstOrDefault()
        //                                         .SelectNodes("article");
        //    foreach (var node in featuredVinesNodes)
        //    {
        //        var vine = VineThumbnail.Parse(node);
        //        vines.Add(vine);
        //    }

        //    return vines;
        //}

        // GET api/vines?url={url}
        //[HttpGet]
        //[GET("api/vines?url={url}")]
        public Vine GetVine(string url)
        {
            string htmlString = VineScopeHtmlRequester(url);

            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(htmlString);

            var indexVideoNode = htmlDocument.GetElementbyId("container").SelectSingleNode("div");
            Vine vine = Vine.Parse(indexVideoNode);

            return vine;
        }

        // GET api/vines?search={query}
        //[HttpGet]
        //[GET("api/vines?search={query}")]
        public IEnumerable<VineThumbnail> GetSearchForVine(string query)
        {
            string htmlString = VineScopeHtmlRequester(string.Format("search.php?query={0}", query));

            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(htmlString);

            var vinesNodes = htmlDocument.GetElementbyId("searchresultdata").SelectNodes("div/article");

            var vines = new List<VineThumbnail>();

            foreach (var node in vinesNodes)
            {
                var vine = VineThumbnail.Parse(node);
                vines.Add(vine);
            }

            return vines;
        }

        //// GET api/albums
        //public VineFull GetRandom()
        //{
        //    throw new NotImplementedException();
        //}

        private string VineScopeHtmlRequester(string resource = "")
        {
            string htmlString = null;

            var request = WebRequest.Create(vineScopeUrl + resource) as HttpWebRequest;

            request.ContentType = "application/json";
            request.Method = "GET";

            var response = request.GetResponse();

            var responseReader = new StreamReader(response.GetResponseStream());

            htmlString = responseReader.ReadToEnd();

            responseReader.Close();

            return htmlString;
        }
    }
}