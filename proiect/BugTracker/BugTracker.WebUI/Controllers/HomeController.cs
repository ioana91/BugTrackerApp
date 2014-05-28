using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BugTracker.Domain.Abstract;
using BugTracker.Domain.Entities;
using WebMatrix.WebData;
using System.Web.Security;

namespace BugTracker.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private IRepository repository;

        public HomeController(IRepository repository)
        {
            this.repository = repository;
        }

        public ActionResult Index()
        {
            repository.Add();
            WebSecurity.CreateUserAndAccount("admin", "admin");
            Roles.CreateRole("administrator");
            Roles.AddUserToRole("admin", "administrator");
            return View();
        }

    }
}
