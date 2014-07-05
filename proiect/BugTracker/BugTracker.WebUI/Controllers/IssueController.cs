using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BugTracker.Domain.Interfaces;
using System.Threading.Tasks;

namespace BugTracker.WebUI.Controllers
{
    [Authorize]
    public class IssueController : Controller
    {
        private IUnitOfWork unitOfWork;

        public IssueController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        //
        // GET: Issue/Index
        public ActionResult Index()
        {
            var issues = unitOfWork.IssueRepository.Get(includeProperties: "Tags");
            return View(issues);
        }

        //
        // GET: Issue/Create
        public ActionResult Create()
        {
            var projects = unitOfWork.ProjectRepository.Get();
            ViewBag.ProjectSelectId = new SelectList(projects, "ProjectId", "Name");

            return View();
        }

        //
        // Edit: Issue/Edit/5
        public async Task<ActionResult> Edit(int id = 0)
        {
            var projects = unitOfWork.ProjectRepository.Get();
            ViewBag.ProjectSelectId = new SelectList(projects, "ProjectId", "Name");

            var issue = await unitOfWork.IssueRepository.GetByIdAsync(id);
            return View(issue);
        }
    }
}