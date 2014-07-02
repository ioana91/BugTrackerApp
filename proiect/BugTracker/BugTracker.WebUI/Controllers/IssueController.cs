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
        public async Task<ActionResult> Index()
        {
            var issues = await unitOfWork.IssueRepository.Get();
            return View(issues);
        }

        //
        // GET: Issue/Create
        public async Task<ActionResult> Create()
        {
            var projects = await unitOfWork.ProjectRepository.Get();
            ViewBag.ProjectSelectId = new SelectList(projects, "ProjectId", "Name");

            return View();
        }

        //
        // Edit: Issue/Edit/5
        public async Task<ActionResult> Edit(int id = 0)
        {
            var projects = await unitOfWork.ProjectRepository.Get();
            ViewBag.ProjectSelectId = new SelectList(projects, "ProjectId", "Name");

            var issue = await unitOfWork.IssueRepository.GetByIdAsync(id);
            return View(issue);
        }
    }
}