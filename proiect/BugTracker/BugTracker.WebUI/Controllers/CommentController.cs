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
    public class CommentController : Controller
    {
        private IUnitOfWork unitOfWork;

        public CommentController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        //
        // GET: Comment/Index
        public async Task<ActionResult> Index(int id)
        {
            var issue = await unitOfWork.IssueRepository.GetByIdAsync(id);

            var comment = new CommentViewModel();
            comment.IssueTitle = issue.Title;
            comment.IssueId = issue.IssueId;
            comment.Comments = issue.Comments.ToList();

            return View(comment);
        }

        //
        // POST: Comment/Add
        [HttpPost]
        public async Task<ActionResult> Add(CommentViewModel model)
        {
            var issue = await unitOfWork.IssueRepository.GetByIdAsync(model.IssueId);

            var comment = new Comment()
            {
                IssueId = model.IssueId,
                Text = model.NewComment,
                Date = DateTime.Now,
                AuthorId = User.Identity.GetUserId()
            };

            unitOfWork.CommentRepository.Insert(comment);
            await unitOfWork.SaveAsync();

            return RedirectToAction("Index", "Comment", new { id = model.IssueId });
        }
    }
}