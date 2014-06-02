using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BugTracker.Domain.Entities;
using BugTracker.Domain.Concrete;
using BugTracker.Domain.Interfaces;

namespace BugTracker.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private IUnitOfWork unitOfWork;

        public HomeController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public ActionResult Index()
        {
            unitOfWork.MilestoneRepository.Insert(new Milestone() { MilestoneId = 2, DueDate = DateTime.Now, Name = "The second One" });
            unitOfWork.SaveAsync();
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