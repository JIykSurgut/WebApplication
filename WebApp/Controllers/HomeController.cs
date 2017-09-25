using System.Web.Mvc;

namespace ZDL.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        [HttpGet]
        public ViewResult Index()
        {
            return View();
        }      
    }
}