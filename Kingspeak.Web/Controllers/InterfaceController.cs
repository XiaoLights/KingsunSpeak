using Kingspeak.AdminController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kingspeak.Web.Controllers
{
    [MyAuthorization(IsAuth = false)]
    public class InterfaceController : Controller
    {
        // GET: Interface
        public ActionResult Dazhi()
        {
            return View();
        }

        public ActionResult Niujin()
        {
            return View();
        }
    }
}