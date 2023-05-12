using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SFMS.Models.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFMS.Controllers {
    [Authorize(Roles = "Admin")]
    public class UserController : Controller {
        
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UserController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager) {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public async Task<IActionResult> List() {
            var users = await _userManager.Users.ToListAsync();
            var userRolesViewModel = new List<UserViewModel>();
            foreach (IdentityUser user in users) {
                var thisViewModel = new UserViewModel();
                thisViewModel.Id = user.Id;
                thisViewModel.Email = user.Email;
               // thisViewModel.FirstName = user.FirstName;
               // thisViewModel.LastName = user.LastName;
                thisViewModel.Roles = await GetUserRoles(user);
                userRolesViewModel.Add(thisViewModel);
            }
            return View(userRolesViewModel);
        }
        private async Task<List<string>> GetUserRoles(IdentityUser user) {
            return new List<string>(await _userManager.GetRolesAsync(user));
        }

        public async Task<IActionResult> Manage(string userId) {
            ViewBag.userId = userId;
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) {
                ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                return View("NotFound");
            }
            ViewBag.UserName = user.UserName;
            var roles = new List<RoleViewModel>();
            foreach (var role in _roleManager.Roles) {
                var roleViewModel = new RoleViewModel{
                    Id = role.Id,
                    RoleName = role.Name
                };
                if (await _userManager.IsInRoleAsync(user, role.Name)) 
                    roleViewModel.Selected = true;
                else 
                    roleViewModel.Selected = false;
                roles.Add(roleViewModel);
            }
            return View(roles);
        }
        [HttpPost]
        public async Task<IActionResult> Manage(List<RoleViewModel> roleViewModels, string userId) {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) {
                return View(roleViewModels);
            }
            var roles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, roles);
            if (!result.Succeeded) {
                ModelState.AddModelError("", "Cannot remove user existing roles.");
                return View(roleViewModels);
            }
            result = await _userManager.AddToRolesAsync(user, roleViewModels.Where(x => x.Selected).Select(y => y.RoleName));
            if (!result.Succeeded) {
                ModelState.AddModelError("", "Cannot add selected roles to user.");
                return View(roleViewModels);
            }
            TempData["msg"] = "Updated successed for " + user.Email;
            return RedirectToAction("List");
        }
    }
}
