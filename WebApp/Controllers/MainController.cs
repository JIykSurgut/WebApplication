using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;
using System.Data.SqlClient;

namespace WebApp.Controllers
{
    public class MainController : Controller
    {

        DbContext dbContext = new DbContext();
        
        // GET: Main
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetJStree()
        {
            var reader = dbContext.GetJStree();
            List<object> tt=  new List<object>();
            while (reader.Read())
            {
                tt.Add(
                new { id = reader.GetInt32(0).ToString(), parent = reader.GetInt32(1).ToString() == "0" ? "#" : reader.GetInt32(1).ToString(), text = reader.GetString(2) });
            }
            return Json(tt,JsonRequestBehavior.AllowGet);
            
        }

        [HttpGet]
        public ActionResult GetArticle(int id)
        {
            SqlParameter[] param = dbContext.GetArticle(id);
            return Json(
                new { html = param[1].Value.ToString() },
                JsonRequestBehavior.AllowGet);
        }

    }
}