using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RoleBasedAuthentication.Models;
using RoleBasedAuthentication.RepoHelpers;
namespace RoleBasedAuthentication.Controllers
{
    [Authorize]
    public class TeacherController : Controller
    {
        private readonly UsersHelper _users;
        private readonly ClassHelper _classHelper;
        private readonly UserManager<ApplicationUser> _userManager;

        public TeacherController(
            UsersHelper users,
            ClassHelper classHelper,
            UserManager<ApplicationUser> userManager
        )
        {
            _users = users;
            _classHelper = classHelper;
            _userManager = userManager;
        }

        #region Teacher
        [HttpGet]
        public IActionResult Dashboard()
        {
            ViewBag.Subjects = _users.GetAllSubjects(0);
            ViewBag.Classes = _classHelper.GetAllClasses();
            if (User.IsInRole("Teacher"))
            {
                var userId = _userManager.GetUserId(User);
                ViewBag.PersonalInfo = _users.GetSingleTeacherDetails(userId);
            }
            return View();
        }
        [HttpGet]
        [Authorize(Roles = "Director")]
        public IActionResult List(TeacherFilterModal modal)
        {
            return PartialView(_users.GetTeachers(modal));
        }

        [HttpGet]
        [Authorize(Roles = "Director")]
        public IActionResult UpdateDetailsPartial(string id)
        {
            TeacherSubjectModal obj = _users.GetSelectedSubject(id);
            ViewBag.Subjects = _users.GetSubjects(obj?.SubjectId);
            ViewBag.Classes = _classHelper.GetClassesDropDown(id);
            return PartialView(obj);
        }

        [HttpPost]
        [Authorize(Roles = "Director")]
        public IActionResult UpdateTeacher(TeacherSubjectModal modal)
        {
            _users.UpdateDetails(modal);
            return Json("Chaged Successfully");
        }

        [HttpPost]
        [Authorize(Roles = "Director")]
        public IActionResult ToggleTeacher(string id, bool status)
        {
            _users.Toggleteacher(id, status);
            return Json("Chaged Successfully");
        }

        [HttpGet]
        public IActionResult SubjectsPartial()
        {
            return PartialView(_users.GetSubjects(0));
        }

        [HttpGet]
        public IActionResult FilterPartial()
        {
            ViewBag.Subjects = _users.GetAllSubjects(0);
            ViewBag.Classes = _classHelper.GetAllClasses();
            return PartialView();
        }

        [HttpGet]
        [Authorize(Roles = "Director")]
        public IActionResult DownloadAsCSV()
        {
            return File(_users.DownloadDetails().ToArray(), "text/csv", "teacherslist.csv");
        }

        [HttpPost]
        public IActionResult ShareDetails(TeacherFilterModal modal)
        {
            _users.SendToEmail(modal);
            return Json("Data sent successfully");
        }

        #endregion

        #region Pagination Operations
        [HttpGet]
        public IActionResult TeacherList()
        {
            ViewBag.Subjects = _users.GetAllSubjects(0);
            ViewBag.Classes = _classHelper.GetAllClasses();
            return View();
        }
        [HttpPost]
        public IActionResult TeacherListPartial(TeacherFilterModal modal)
        {
            ViewBag.PageSize = modal.pageSize;
            return PartialView(_users.GetPaginatedTeachersSP(modal));
        }

        #endregion

    }
}
    