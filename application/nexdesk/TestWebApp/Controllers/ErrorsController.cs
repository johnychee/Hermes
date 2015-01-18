using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TestWebApp.Controllers
{
    public sealed class ErrorsController : Controller
    {
        public ActionResult NotFound()
        {
            ActionResult result;

            object model = Request.Url.PathAndQuery;

            if (!Request.IsAjaxRequest())
                result = View("NotFound");
            else
                result = View("NotFound"); //PartialView("_NotFound", model);

            return result;
        }
    }
}