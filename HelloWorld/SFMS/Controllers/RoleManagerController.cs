using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Threading.Tasks;

namespace SFMS.Controllers {
    [Authorize(Roles = "Admin")]
    public class RoleManagerController : Controller {
        private readonly RoleManager<IdentityRole> _roleManager;
        public RoleManagerController(RoleManager<IdentityRole> roleManager) {
            _roleManager = roleManager;
        }
        public async Task<IActionResult> List() {
            var roles = await _roleManager.Roles.ToListAsync();
            return View(roles);
        }
        [HttpPost]
        public async Task<IActionResult> EntryRole(string roleName) {
            if (roleName != null) {
                var role = new IdentityRole(roleName.Trim());
                role.ConcurrencyStamp = DateTime.Now.ToString();
                await _roleManager.CreateAsync(role);
                TempData["msg"] = "Saving success for " + roleName;
            }
            return RedirectToAction("List");
        }
        [HttpPost]
        public async Task<IActionResult> DeleteRole(string id) {
            var role=await _roleManager.FindByIdAsync(id);
            if (role != null && (role.Name.Equals("SystemAdmin") ||role.Name.Equals("Admin") || role.Name.Equals("Teacher") || role.Name.Equals("Student"))) {
                TempData["msg"] = "Oh,sorry CAN NOT DELETE this " + role.Name + " role .";
            }
               else if (role != null) {
                await _roleManager.DeleteAsync(role);
                TempData["msg"] = "Delete process is successed for " + role.Name;
            }
            return RedirectToAction("List");
        }
    }
}
