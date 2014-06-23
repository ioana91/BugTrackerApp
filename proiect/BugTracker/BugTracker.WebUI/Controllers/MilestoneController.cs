using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BugTracker.Domain.Interfaces;

namespace BugTracker.WebUI.Controllers
{
    public class MilestoneController : Controller
    {
        private IUnitOfWork unitOfWork;

        public MilestoneController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        //
        // GET: Milestone
        public ActionResult Create()
        {
            return View();
        }
    }
}