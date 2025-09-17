using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
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
        public TeacherController(
            UsersHelper users,
            ClassHelper classHelper
        )
        {
            _users = users;
            _classHelper = classHelper;
        }

        #region Teacher
        [HttpGet]
        public IActionResult Dashboard()
        {
            ViewBag.Subjects = _users.GetAllSubjects(0);
            ViewBag.Classes = _classHelper.GetAllClasses();
            return View();
        }
        [HttpGet]
        public IActionResult List(TeacherFilterModal modal)
        {
            return PartialView(_users.GetTeachers(modal));
        }

        [HttpGet]
        public IActionResult UpdateDetailsPartial(string id)
        {
            TeacherSubjectModal obj = _users.GetSelectedSubject(id);
            ViewBag.Subjects = _users.GetSubjects(obj?.SubjectId);
            ViewBag.Classes = _classHelper.GetClassesDropDown(id);
            return PartialView(obj);
        }

        [HttpPost]
        public IActionResult UpdateTeacher(TeacherSubjectModal modal)
        {
            _users.UpdateDetails(modal);
            return Json("Chaged Successfully");
        }

        [HttpPost]
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
