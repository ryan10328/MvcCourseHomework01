using MvcHomework01.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcHomework01.Controllers
{
    public class VWController : Controller
    {
        private CrmEntities db = new CrmEntities();
        // GET: VW
        public ActionResult Index()
        {
            var result = db.VWCustomerInformations.ToList();
            return View(result);
        }
    }
}