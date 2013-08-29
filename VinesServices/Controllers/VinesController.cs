namespace VinesServices.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Http;
    using VinesServices.Models;

    public class VinesController : ApiController
    {
        // GET api/albums
        public IQueryable<VinesList> Get()
        {
            throw new NotImplementedException();
        }

        // GET api/albums
        public VineFull Get(string url)
        {
            throw new NotImplementedException();
        }

        // GET api/albums
        public VineFull Search(string query)
        {
            throw new NotImplementedException();
        }

        // GET api/albums
        public VineFull GetRandom()
        {
            throw new NotImplementedException();
        }
    }
}
