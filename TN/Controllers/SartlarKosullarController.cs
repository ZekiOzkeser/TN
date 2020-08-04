using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TN.Controllers
{
    public class SartlarKosullarController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GizlilikPolitikamiz()
        {
            return View();
        }

        public ActionResult Kurallar()
        {
            return View();
        }

    }
}