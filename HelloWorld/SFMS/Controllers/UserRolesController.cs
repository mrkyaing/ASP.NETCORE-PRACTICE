using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SFMS.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFMS.Controllers {
    [Authorize(Roles = "Admin")]
    public class UserRolesController : Controller {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UserRolesController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager) {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public async Task<IActionResult> List() {
            var users = await _userManager.Users.ToListAsync();
            var userRolesViewModel = new List<UserRolesViewModel>();
            foreach (IdentityUser user in users) {
                var thisViewModel = new UserRolesViewModel();
                thisViewModel.UserId = user.Id;
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
    }
}
