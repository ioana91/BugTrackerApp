using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.Net.Mail;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using BugTracker.Domain.Interfaces;
using BugTracker.Domain.Entities;
using BugTracker.WebUI.Models;

namespace BugTracker.WebUI.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private IUnitOfWork unitOfWork;
        private UserManager<ApplicationUser> userManager;

        public UserController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(unitOfWork.GetContext));
        }

        //
        // GET: User/Add
        public ActionResult Add()
        {
            return View();
        }

        //
        // POST: User/Add
        [HttpPost]
        public async Task<ActionResult> Add(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var delimiters = new char[] { ' ', '-' };
                var firstNameParts = model.FirstName.Split(delimiters);
                var lastNameParts = model.LastName.Split(delimiters);

                var password = GeneratePassword();

                var userName = firstNameParts[0].ToLower() + '.' + lastNameParts[0].ToLower();
                var user = new ApplicationUser()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    UserName = userName
                };
                var result = await userManager.CreateAsync(user, password);

                await SendMail(model, userName, password);

                if (result.Succeeded)
                {
                    var currentUser = userManager.FindByName(user.UserName);
                    userManager.AddToRole(currentUser.Id, "Employee");

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    AddErrors(result);
                }
            }

            return View(model);
        }

        private async Task SendMail(UserViewModel model, string userName, string password)
        {
            var message = new MailMessage();
            message.From = new MailAddress("bugtrackerapp@gmail.com", "BugTracker App");
            message.To.Add(new MailAddress(model.Email));

            var userId = User.Identity.GetUserId();
            var user = await unitOfWork.UserRepository.GetByIdAsync(userId);

            message.Subject = "Bug Tracker Team";
            message.Body = "The user " + user.DisplayName + " has added you to his team." + System.Environment.NewLine +
                "Your current username is " + userName + " and your password is " + password;

            SmtpClient client = new SmtpClient();
            client.Send(message);
        }

        private string GeneratePassword()
        {
            int minPasswordLength = 8;
            int maxPasswordLength = 10;
            string passwordCharacters = "abcdefghijklmnopqrstuvwxyz0123456789#+@&$ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            var password = "";
            Random random = new Random();
            var passwordLength = random.Next(minPasswordLength, maxPasswordLength + 1);

            for (int i = 0; i < passwordLength; i++)
            {
                var index = random.Next(0, passwordCharacters.Length - 1);
                password += passwordCharacters[index];
            }

            return password;
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
    }
}