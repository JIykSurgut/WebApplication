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
        public ActionResult GetMenu()
        {
            var reader = dbContext.GetMenu();
            List<object> itemList = new List<object>();
            while (reader.Read())
            {
                itemList.Add(
                new { id = reader.GetInt32(0).ToString(),
                      parent = reader.GetInt32(1).ToString() == "0" ? "#" : reader.GetInt32(1).ToString(),
                      text = reader.GetString(2)
                    });
            }
            return Json(itemList, JsonRequestBehavior.AllowGet);           
        }

        
        [HttpGet]
        public ActionResult GetContent(int id)
        {
            SqlParameter[] param = dbContext.GetContent(id);
            return Json(
                new { content = param[1].Value.ToString(),
                      codeId = param[2].Value.ToString()},
                JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetContentCode(int id)
        {
            SqlParameter[] param = dbContext.GetContentCode(id);
            return Json(
                new
                {
                    contentCode = param[1].Value.ToString()
                },
                JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult CodeView()
        {
            return View();
        }


        [HttpGet]
        public ActionResult GetMenuCode(int codeId)
        {
            var reader = dbContext.GetMenuCode(codeId);
            List<object> itemList = new List<object>();
            while (reader.Read())
            {
                itemList.Add(
                new { id = reader.GetInt32(0).ToString(),
                      parent = reader.GetInt32(1).ToString() == "0" ? "#" : reader.GetInt32(1).ToString(),
                      text = reader.GetString(2).ToString(),
                      icon = "/Content/"+reader.GetString(3).ToString()
                });
            }
            return Json(itemList, JsonRequestBehavior.AllowGet);

        }


    }
}