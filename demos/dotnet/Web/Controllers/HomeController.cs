namespace Web.Controllers
{
    using System.Web.Http;

    public class HomeController : ApiController
    {
        [HttpGet, HttpPost]
        public IHttpActionResult Index()
        {
            var title = "Home Page";

            return Ok(title);
        }
    }
}
