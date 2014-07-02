using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BugTracker.Domain.Interfaces;
using BugTracker.Domain.Entities;
using System.Threading.Tasks;

namespace BugTracker.WebUI.Controllers
{
    [Authorize]
    public class ProjectController : Controller
    {
        private IUnitOfWork unitOfWork;

        public ProjectController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        //
        // GET: Project/Index
        public async Task<ActionResult> Index()
        {
            var projects = await unitOfWork.ProjectRepository.Get();
            return View();
        }

        //
        // GET: Project/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: Project/Create
        [HttpPost]
        public ActionResult Create(Project project)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.ProjectRepository.Insert(project);
                unitOfWork.SaveAsync();
                return RedirectToAction("Index", "Home");
            }

            return View(project);
        }
    }
}