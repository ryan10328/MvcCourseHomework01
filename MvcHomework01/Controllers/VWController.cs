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

        public ActionResult Complex()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Complex(Student student)
        {
            return View();
        }



    }


    public class Student
    {
        public int StudentId { get; set; }
        public string Name { get; set; }
        public string Summary { get; set; }
        public List<WorkExperience> WorkExpereriences { get; set; }
    }

    public class WorkExperience
    {
        public int JobId { get; set; }
        public string JobName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}