using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BugTracker.Domain.Entities;
using BugTracker.Domain.Concrete;
namespace BugTracker.WebUI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            BugTrackerDBContext context = new BugTrackerDBContext();
            context.Milestones.Add(new Milestone() { MilestoneId = 1, DueDate = DateTime.Now, Name = "FirstMilestone" });

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}