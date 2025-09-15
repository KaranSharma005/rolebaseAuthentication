using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoleBasedAuthentication.RepoHelpers;

namespace RoleBasedAuthentication.Controllers
{
    [Authorize]
    public class StudentController : Controller
    {
        private readonly UsersHelper _users;
        public StudentController(
            UsersHelper users
        )
        {
            _users = users;
        }

        #region Student

        public IActionResult Dashboard()
        {
            return View();
        }
        public async Task<IActionResult> List()
        {
            var result = await _users.GetStudents();
            return PartialView("_List", result);
        }
        #endregion
    }
}
