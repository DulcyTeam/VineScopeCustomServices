namespace VinesServices.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using HtmlAgilityPack;
    using VinesServices.Models;

    public class VinesController : ApiController
    {
        private const string VineScopeUrl = "http://vinescope.com/";

        [HttpGet]
        [ActionName("all")]
        public HttpResponseMessage Get()
        {
            string htmlString = VineScopeHtmlRequester();

            if (htmlString == null)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Server is down");
            }

            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(htmlString);

            var indexVideoNode = htmlDocument.GetElementbyId("container").SelectSingleNode("div");
            VineThumbnail indexMainVine = Vine.Parse(indexVideoNode);
            indexMainVine.Url = Vine.GetUrl(indexMainVine);
            var vines = new List<VineThumbnail>();
            vines.Add(indexMainVine);

            var featuredVinesNodes = htmlDocument.GetElementbyId("container")
                                                 .SelectNodes("div/div/div")
                                                 .Where(n => n.Attributes["class"] != null && n.Attributes["class"].Value == "thumb-list")
                                                 .FirstOrDefault()
                                                 .SelectNodes("article");
            foreach (var node in featuredVinesNodes)
            {
                var vine = VineThumbnail.Parse(node);
                vine.Url = Vine.GetUrl(vine);
                vines.Add(vine);
            }

            return this.Request.CreateResponse(HttpStatusCode.OK, vines);
        }

        [HttpGet]
        [ActionName("vine")]
        public HttpResponseMessage GetVine(string url)
        {
            string htmlString = VineScopeHtmlRequester(url);

            if (htmlString == null)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Server is down");
            }

            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(htmlString);

            var indexVideoNode = htmlDocument.GetElementbyId("container").SelectSingleNode("div");

            Vine vine = null;
            
            try
            {
                vine = Vine.Parse(indexVideoNode);
            }
            catch (Exception)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.NotFound, "Item was not found");
            }

            return this.Request.CreateResponse(HttpStatusCode.OK, vine);
        }

        [HttpGet]
        [ActionName("search")]
        public HttpResponseMessage GetSearchForVine(string query)
        {
            string htmlString = VineScopeHtmlRequester(string.Format("search.php?query={0}", query));

            if (htmlString == null)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Server is down");
            }

            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(htmlString);

            var vinesNodes = htmlDocument.GetElementbyId("searchresultdata").SelectNodes("div/article");

            if (vinesNodes == null)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.NotFound, "Item not found");
            }

            var vines = new List<VineThumbnail>();

            foreach (var node in vinesNodes)
            {
                var vine = VineThumbnail.Parse(node);
                vines.Add(vine);
            }

            return this.Request.CreateResponse(HttpStatusCode.OK, vines);
        }

        [HttpGet]
        [ActionName("random")]
        public HttpResponseMessage GetRandom()
        {
            string htmlString = VineScopeHtmlRequester("random.php");

            if (htmlString == null)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Server is down");
            }

            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(htmlString);

            var indexVideoNode = htmlDocument.GetElementbyId("container").SelectSingleNode("div");
            Vine vine = Vine.Parse(indexVideoNode);
            vine.Url = Vine.GetUrl(vine);

            return this.Request.CreateResponse(HttpStatusCode.OK, vine);
        }

        private string VineScopeHtmlRequester(string resource = "")
        {
            string htmlString;

            var request = WebRequest.Create(VineScopeUrl + resource) as HttpWebRequest;

            request.ContentType = "application/json";
            request.Method = "GET";

            WebResponse response;

            try
            {
                response = request.GetResponse();
            }
            catch (Exception)
            {
                return null;
            }

            var responseReader = new StreamReader(response.GetResponseStream());

            htmlString = responseReader.ReadToEnd();

            responseReader.Close();

            return htmlString;
        }
    }
}