using ShackUp.Data.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShackUp.UI.Controllers
{
    public class HomeController : Controller
    {
        
        public ActionResult Index()
        {
            var model = ListingRepositoryFactory.GetRepository().GetRecent();
            return View(model);
        }
    }
}