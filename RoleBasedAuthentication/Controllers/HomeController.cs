using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoleBasedAuthentication.Models;
using RoleBasedAuthentication.RepoHelpers;
using System.Diagnostics;


namespace RoleBasedAuthentication.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UsersHelper _users;
        private readonly SpaHelper _help;
        public HomeController(
            ILogger<HomeController> logger,
            UsersHelper users,
            SpaHelper help)
        {
            _logger = logger;
            _users = users;
            _help = help;
        }

        #region Student

        public async Task<IActionResult> Index()
        {
            return View();
        }

        //StudentPartial
        [HttpGet]
        public IActionResult Table()
        {
            return PartialView(_help.Get());
        }

        //AddStudent
        [HttpGet]
        public IActionResult AddUser()
        {
            return PartialView();
        }

        //AddStudent
        [HttpPost]
        public IActionResult AddUser(MyModal mdl)
        {
            if (!ModelState.IsValid)
            {
                return View(mdl);
            }
            _help.Add(mdl);
            return Json("Data sent successfully");
        }

        [HttpGet]
        public IActionResult UpdateForm(int id)
        {
            var studentData = _help.getStudent(id);
            return PartialView(studentData);
        }

        //DeleteStudent
        [HttpGet]
        public IActionResult RemoveOne(int id)
        {
            _help.delete(id);
            return View("Index");
        }

        #endregion

        public IActionResult Privacy()
        {
            return View();
        }


        [HttpPost]
        public IActionResult UpdateChanges(MyModal mdl)
        {
            if (!ModelState.IsValid)
            {
                return View(mdl);
            }
            _help.Update(mdl);
            return PartialView("Table", _help.Get());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
