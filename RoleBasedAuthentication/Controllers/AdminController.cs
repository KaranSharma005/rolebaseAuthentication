using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RoleBasedAuthentication.Models;
using RoleBasedAuthentication.RepoHelpers;

namespace RoleBasedAuthentication.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AdminController(UserManager<ApplicationUser> userManager,
                               SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task<IActionResult> LoginAs(string id)
        {
            var user = await _userManager.FindByIdAsync(id)
                     ?? await _userManager.FindByEmailAsync(id);

            if (user == null)
                return NotFound("User not found");

            await _signInManager.SignOutAsync();

            await _signInManager.SignInAsync(user, isPersistent: false);

            return RedirectToAction("Dashboard", "Teacher");
        }
    }
}
