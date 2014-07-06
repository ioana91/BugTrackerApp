using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BugTracker.Domain.Interfaces;
using System.Threading.Tasks;
using BugTracker.WebUI.Models;
using BugTracker.Domain.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;

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
            var user = unitOfWork.UserRepository.Get(filter: u => u.UserName == User.Identity.Name, includeProperties: "Projects");
            var projects = user.FirstOrDefault().Projects.ToList();

            ViewBag.ProjectSelectId = new SelectList(projects, "ProjectId", "Name");

            return View();
        }

        //
        // POST: Issue/Create
        [HttpPost]
        public async Task<ActionResult> Create(IssueViewModel model)
        {
            string color;
            IssuePriority priority;

            GetColorAndPriority(model.Priority, out color, out priority);
            var type = GetType(model.Type);
            var milestone = unitOfWork.MilestoneRepository.Get(filter: m => m.MilestoneId == model.MilestoneSelectId)
                .FirstOrDefault();
            var tags = GetSelectedTags(model.Tags, model.ProjectSelectId);
            
            var issue = new Issue() { Title = model.Title, Description = model.Description, Priority = priority, 
                Color = color, Type = type, Status = IssueStatus.Open, ProjectId = model.ProjectSelectId, 
                Milestone = milestone, Tags = tags};

            unitOfWork.IssueRepository.Insert(issue);
            await unitOfWork.SaveAsync();

            return RedirectToAction("Index", "Issue");
        }

        //
        // GET: Issue/Edit/5
        public async Task<ActionResult> Edit(int id = 0)
        {
            var issue = await unitOfWork.IssueRepository.GetByIdAsync(id);

            var projects = new List<Project>() { issue.Project };
            ViewBag.ProjectSelectId = new SelectList(projects, "ProjectId", "Name", issue.ProjectId);

            var milestones = unitOfWork.MilestoneRepository.Get(filter: m => m.ProjectId == issue.ProjectId);
            ViewBag.MilestoneSelectId = new SelectList(milestones, "MilestoneId", "Name", issue.MilestoneId);

            var tags = unitOfWork.ProjectRepository
                .Get(filter: p => p.ProjectId == issue.ProjectId,
                includeProperties: "Tags")
                .FirstOrDefault()
                .Tags;
            var tagsArray = tags
                .Select(t => new { id = t.TagId, name = t.Name })
                .ToArray();
            ViewBag.AllTags = tagsArray;

            var selectedTags = issue.Tags.Select(t => t.Name).ToArray();
            ViewBag.SelectedTags = selectedTags;

            if (issue.Responsible != null)
            {
                var responsible = new List<ApplicationUser>() { issue.Responsible };
                ViewBag.ResponsibleId = new SelectList(responsible, "Id", "DisplayName", issue.ResponsibleId);
            }
            else
            {
                var users = issue.Project.Users;
                ViewBag.ResponsibleId = new SelectList(users, "Id", "DisplayName");
            }

            return View(issue);
        }

        //
        // POST: Issue/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(EditIssueViewModel model)
        {
            string color;
            IssuePriority priority;
            GetColorAndPriority(model.Priority, out color, out priority);
            var status = GetStatus(model.Status);

            var issue = await unitOfWork.IssueRepository.GetByIdAsync(model.IssueId);

            issue.Title = model.Title;
            issue.Description = model.Description;
            issue.Color = color;
            issue.Priority = priority;
            issue.Status = status;

            if (model.ResponsibleId != null)
            {
                issue.ResponsibleId = model.ResponsibleId;
            }

            var tags = GetSelectedTags(model.EditTags, issue.ProjectId);
            issue.Tags = tags;

            unitOfWork.IssueRepository.Update(issue);
            await unitOfWork.SaveAsync();

            return RedirectToAction("Index", "Issue");
        }

        //
        // GET: Issue/Assign/5
        public async Task<ActionResult> Assign(int id)
        {
            var issue = await unitOfWork.IssueRepository.GetByIdAsync(id);

            issue.ResponsibleId = User.Identity.GetUserId();

            unitOfWork.IssueRepository.Update(issue);
            await unitOfWork.SaveAsync();

            return RedirectToAction("Index", "Issue");
        }

        //
        // GET: Issue/Unassign/5
        public async Task<ActionResult> Unassign(int id)
        {
            var issue = await unitOfWork.IssueRepository.GetByIdAsync(id);

            issue.Responsible = null;

            unitOfWork.IssueRepository.Update(issue);
            await unitOfWork.SaveAsync();

            return RedirectToAction("Index", "Issue");
        }

        //
        // GET: Issue/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var issue = await unitOfWork.IssueRepository.GetByIdAsync(id);

            if (issue.Comments != null)
            {
                issue.Comments.Clear();
            }
            if (issue.Tags != null)
            {
                issue.Tags.Clear();
            }
            if (issue.UsersInvolved != null)
            {
                issue.UsersInvolved.Clear();
            }

            unitOfWork.IssueRepository.Delete(id);
            await unitOfWork.SaveAsync();

            return RedirectToAction("Index", "Issue");
        }

        [HttpGet]
        public JsonResult GetMilestones(int projectId)
        {
            var milestones = unitOfWork.ProjectRepository
                .Get(filter: p => p.ProjectId == projectId,
                includeProperties: "Milestones")
                .FirstOrDefault()
                .Milestones;
            var milestonesArray = milestones
                .OrderBy(m => m.DueDate)
                .Select(m => new { m.MilestoneId, m.Name })
                .ToArray();

            return Json(milestonesArray, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetTags(int projectId)
        {
            var tags = unitOfWork.ProjectRepository
                .Get(filter: p => p.ProjectId == projectId,
                includeProperties: "Tags")
                .FirstOrDefault()
                .Tags;
            var tagsArray = tags
                .Select(t => new { id = t.TagId, name = t.Name })
                .ToArray();

            return Json(tagsArray, JsonRequestBehavior.AllowGet);
        }

        private void GetColorAndPriority(string modelPriority, out string color, out IssuePriority priority)
        {
            if (modelPriority == IssuePriority.Medium.ToString())
            {
                color =  "info";
                priority = IssuePriority.Medium;
            }
            else if(modelPriority == IssuePriority.Low.ToString())
            {
                color = "success";
                priority = IssuePriority.Low;
            }
            else if (modelPriority == IssuePriority.High.ToString())
            {
                color = "warning";
                priority = IssuePriority.High;
            }
            else
            {
                color = "danger";
                priority = IssuePriority.Critical;
            }
        }

        private IssueType GetType(string type)
        {
            if (type == IssueType.Bug.ToString())
            {
                return IssueType.Bug;
            }
            else
            {
                return IssueType.Feature;
            }
        }

        private List<Tag> GetSelectedTags(List<string> tags, int projectId)
        {
            List<Tag> selectedTags = new List<Tag>();

            var existingTags = unitOfWork.TagRepository.Get(filter: t => t.ProjectId == projectId && tags.Contains(t.Name));

            foreach (var tag in tags)
            {
                var existingTag = existingTags.FirstOrDefault(t => t.Name == tag);

                if (existingTag == null)
                {
                    existingTag = new Tag() { Name = tag, ProjectId = projectId };
                }

                selectedTags.Add(existingTag);
            }

            return selectedTags;
        }

        private IssueStatus GetStatus(string status)
        {
            if (status == "3")
            {
                return IssueStatus.Closed;
            }
            else if (status == "1")
            {
                return IssueStatus.InProgress;
            }
            else if (status == "2")
            {
                return IssueStatus.InTesting;
            }
            else if (status == "0")
            {
                return IssueStatus.Open;
            }
            else
            {
                return IssueStatus.Unsolvable;
            }
        }
    }
}