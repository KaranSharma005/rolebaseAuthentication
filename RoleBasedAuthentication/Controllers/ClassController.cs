using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoleBasedAuthentication.Models;
using RoleBasedAuthentication.RepoHelpers;
namespace RoleBasedAuthentication.Controllers
{
    [Authorize]
    public class ClassController : Controller
    {
        private readonly ClassHelper _classhelper;
        public ClassController(
            ClassHelper classhelper
        )
        {
            _classhelper = classhelper;
        }
        #region Class

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ClassModalPartial()
        {
            return PartialView();
        }

        [HttpPost]
        public IActionResult AddClass(ClassModal mdl)
        {
            _classhelper.addClass(mdl);
            return Json("Done");
        }

        [HttpGet]
        public IActionResult ListClass()
        {
            return PartialView(_classhelper.GetList());
        }

        [HttpPost]
        public IActionResult DeleteClass(int id)
        {
            _classhelper.DeleteClass(id);
            return Json("Deletion completed");
        }

        [HttpGet]
        public IActionResult UpdatePartial(int id)
        {
            return PartialView(_classhelper.GetDetails(id));
        }

        [HttpPost]
        public IActionResult Update(ClassModal mdl)
        {
            _classhelper.UpdatedClass(mdl);
            return Json("Updated Successfully");
        }

        [HttpGet]
        public IActionResult ClassDropdownpartial()
        {
            return PartialView(_classhelper.GetClassesDropDown(""));
        }
        #endregion
    }
}
