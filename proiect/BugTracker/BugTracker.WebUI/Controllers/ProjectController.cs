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
using BugTracker.WebUI.Models;

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

        public async Task<JsonResult> GetResponsibleDetails(int projectId)
        {
            var project = await unitOfWork.ProjectRepository.GetByIdAsync(projectId);

            string[][] userDetailsList = new string[project.Users.Count + 1][];
            for (int i = 0; i < project.Users.Count + 1; i++)
            {
                userDetailsList[i] = new string[2];
            }

            var assignedIssues = 0;
            for (int i = 0; i < project.Users.Count; i++)
            {
                var responsible = project.Users.ElementAt(i);
                if (responsible != null)
                {
                    userDetailsList[i][0] = responsible.DisplayName;
                    userDetailsList[i][1] = responsible.Issues.Count.ToString();
                    assignedIssues += responsible.Issues.Count;
                }
            }

            userDetailsList[project.Users.Count][0] = "Unassigned";
            userDetailsList[project.Users.Count][1] = (project.Issues.Count - assignedIssues).ToString();

            return Json(userDetailsList, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> Reports(int id)
        {
            var project = await unitOfWork.ProjectRepository.GetByIdAsync(id);
            var reportDetails = new ReportViewModel();

            //set project name
            reportDetails.ProjectName = project.Name;
            reportDetails.ProjectId = id;

            //data for status chart
            reportDetails.OpenName = "Open";
            reportDetails.OpenIssues = project.Issues.Count(i => i.Status == IssueStatus.Open);

            reportDetails.ClosedName = "Closed";
            reportDetails.ClosedIssues = project.Issues.Count(i => i.Status == IssueStatus.Closed);

            reportDetails.InProgressName = "In progress";
            reportDetails.InProgressIssues = project.Issues.Count(i => i.Status == IssueStatus.InProgress);

            reportDetails.InTestingName = "In testing";
            reportDetails.InTestingIssues = project.Issues.Count(i => i.Status == IssueStatus.InTesting);

            reportDetails.UnsolvableName = "Unsolvable";
            reportDetails.UnsolvableIssues = project.Issues.Count(i => i.Status == IssueStatus.Unsolvable);

            //data for priority chart
            reportDetails.Low = "Low";
            reportDetails.LowIssues = project.Issues.Count(i => i.Priority == IssuePriority.Low);

            reportDetails.Medium = "Medium";
            reportDetails.MediumIssues = project.Issues.Count(i => i.Priority == IssuePriority.Medium);

            reportDetails.Critical = "Critical";
            reportDetails.CriticalIssues = project.Issues.Count(i => i.Priority == IssuePriority.Critical);

            reportDetails.High = "High";
            reportDetails.HighIssues = project.Issues.Count(i => i.Priority == IssuePriority.High);

            //data for type chart
            reportDetails.Bug = "Bug";
            reportDetails.BugIssues = project.Issues.Count(i => i.Type == IssueType.Bug);

            reportDetails.Feature = "Feature";
            reportDetails.FeatureIssues = project.Issues.Count(i => i.Type == IssueType.Feature);

            return View(reportDetails);
        }
    }
}