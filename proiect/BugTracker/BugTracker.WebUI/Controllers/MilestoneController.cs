using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BugTracker.Domain.Interfaces;
using BugTracker.Domain.Entities;
using BugTracker.WebUI.Models;
using System.Threading.Tasks;

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
        // GET: Milestone/Index
        public ActionResult Index()
        {
            var milestones = unitOfWork.MilestoneRepository.Get();
            var projects = milestones.Select(m => m.Project).Distinct().ToList();
            var milestoneDetailsList = new List<MilestoneDetails>();

            foreach (var project in projects)
            {
                foreach (var milestone in project.Milestones)
                {
                    var milestoneDetails = new MilestoneDetails() { ProjectId = project.ProjectId, 
                        MilestoneId = milestone.MilestoneId, Name = milestone.Name, Date = milestone.DueDate};

                    var closedIssues = 0;
                    var inProgressIssues = 0;
                    var inTestingIssues = 0;
                    var openIssues = 0;
                    var unsolvableIssues = 0;

                    foreach (var issue in milestone.Issues)
                    {
                        if (issue.Status == Domain.Entities.IssueStatus.Closed)
                        {
                            closedIssues++;
                        }
                        else if (issue.Status == Domain.Entities.IssueStatus.InProgress)
                        {
                            inProgressIssues++;
                        }
                        else if (issue.Status == Domain.Entities.IssueStatus.InTesting)
                        {
                            inTestingIssues++;
                        }
                        else if (issue.Status == Domain.Entities.IssueStatus.Open)
                        {
                            openIssues++;
                        }
                        else
                        {
                            unsolvableIssues++;
                        }
                    }

                    var sum = closedIssues + inProgressIssues + inTestingIssues + openIssues + unsolvableIssues;

                    milestoneDetails.ClosedIssues = 100 * closedIssues / sum ;
                    milestoneDetails.InProgressIssues = 100 * inProgressIssues / sum;
                    milestoneDetails.InTestingIssues = 100 * inTestingIssues / sum;
                    milestoneDetails.OpenIssues = 100 * openIssues / sum;
                    milestoneDetails.UnsolvableIssues = 100 * unsolvableIssues / sum;

                    milestoneDetailsList.Add(milestoneDetails);
                }
            }

            var milestoneViewModel = new MilestoneIndexViewModel() { MilestoneDetails = milestoneDetailsList, Projects = projects };
            return View(milestoneViewModel);
        }

        //
        // GET: Milestone/Create
        public ActionResult Create()
        {
            var user = unitOfWork.UserRepository.Get(filter: u => u.UserName == User.Identity.Name, includeProperties: "Projects");
            var projects = user.FirstOrDefault().Projects.ToList();
            
            ViewBag.ProjectSelectId = new SelectList(projects, "ProjectId", "Name");

            return View();
        }

        //
        //POST: Milestone/Create
        [HttpPost]
        public async Task<ActionResult> Create(MilestoneViewModel model)
        {
            var dueDate = new DateTime(model.Year, model.Month + 1, model.Date);
            var projectId = int.Parse(((string[])model.Project)[0]);

            var milestone = new Milestone() { DueDate = dueDate, Name = model.Name, ProjectId = projectId };
            unitOfWork.MilestoneRepository.Insert(milestone);
            await unitOfWork.SaveAsync();

            return RedirectToAction("Index", "Issue");
        }
    }
}