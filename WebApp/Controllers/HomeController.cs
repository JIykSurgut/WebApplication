using System.Web.Mvc;

namespace ZDL.Controllers
{ 
    public class HomeController : Controller
    {
        [HttpGet]
        public ViewResult Index()
        {
            return View();
        }      
    }
}