using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BugTracker.Domain.Interfaces;
using BugTracker.Domain.Entities;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BugTracker.WebUI.Controllers
{
    [Authorize]
    public class ProjectController : Controller
    {
        public class ProjectEmployees
        {
            public string ProjectName { get; set; }
            public List<string> Employees { get; set; }
        }

        private IUnitOfWork unitOfWork;

        public ProjectController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        //
        // GET: Project/Index
        public ActionResult Index()
        {
            var user = unitOfWork.UserRepository.Get(filter: u => u.UserName == User.Identity.Name, includeProperties: "Projects");
            return View(user.FirstOrDefault().Projects.ToList());
        }

        //
        // POST: Project/Index
        [HttpPost]
        public async Task<ActionResult> Index(List<ProjectEmployees> projectsArray)
        {
            foreach (var projectDetails in projectsArray)
            {
                var project = unitOfWork.ProjectRepository.Get(filter: p => p.Name == projectDetails.ProjectName, 
                    includeProperties: "Users").FirstOrDefault();
                project.Users.Clear();

                foreach (var employee in projectDetails.Employees)
                {
                    if (employee != "")
                    {
                        var parts = employee.Split(new char[] { ' ' });
                        var firstName = parts[0];
                        var lastName = parts[1];
                        var user = unitOfWork.UserRepository.Get(filter: u => u.FirstName == firstName && 
                            u.LastName == lastName).FirstOrDefault();
                        project.Users.Add(user);
                    }
                }

                var manager = unitOfWork.UserRepository.Get(u => u.UserName == User.Identity.Name).FirstOrDefault();
                project.Users.Add(manager);
            }

            await unitOfWork.SaveAsync();

            return RedirectToAction("Index", "Issue");
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
        public async Task<ActionResult> Create(Project project)
        {
            if (ModelState.IsValid)
            {
                string managerUserName = User.Identity.Name;
                var user = unitOfWork.UserRepository.Get(filter: u => u.UserName == managerUserName);
                project.Users.Add(user.FirstOrDefault());

                unitOfWork.ProjectRepository.Insert(project);
                await unitOfWork.SaveAsync();

                return RedirectToAction("Index", "Project");
            }

            return View(project);
        }

        [HttpGet]
        public JsonResult GetEmployees()
        {
            var managerId = User.Identity.GetUserId();
            var employees = unitOfWork.UserRepository.Get(filter: u => u.ManagerId == managerId)
                .Select(e => e.DisplayName)
                .ToArray();

            return Json(employees, JsonRequestBehavior.AllowGet);
        }
    }
}