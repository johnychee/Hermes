using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestWebApp.Models;

namespace TestWebApp.Controllers
{
    public class SearchController : Controller
    {
        //
        // POST: /Search/
        [HttpPost]
        [Authorize]
        public ActionResult GlobalSearch(FormCollection fc)
        {
            return View();
        }
    }
}