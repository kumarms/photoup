using AttributeRouting.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PhotoUp.Controllers
{
    public class SessionController : Controller
    {
        [GET("/session")]
        public ActionResult Index()
        {
            return View();
        }

        [POST("/session")]
        public ActionResult Create(string user)
        {
            Session["user"] = user;

            return Redirect("/");
        }

        [POST("/session/destroy")]
        public ActionResult Destroy()
        {
            Session.Abandon();

            return Redirect("/");
        }
    }
}
