using IdentityAppPractice.Data;
using IdentityAppPractice.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityAppPractice.Controllers
{
    public class RoleController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleController(ApplicationDbContext db, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public IActionResult Index()
        {
            var roles = _db.Roles.ToList();
            return View(roles);
        }
        [HttpGet]
        public IActionResult Upsert(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return View();
            }
            else
            {
                var user = _db.Roles.FirstOrDefault(x => x.Id == id);
                return View(user);
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(IdentityRole role)
        {
            if (await _roleManager.RoleExistsAsync(role.Name))
            {
                return RedirectToAction("Index");
            }
            if (string.IsNullOrEmpty(role.Id))
            {
                await _roleManager.CreateAsync(new IdentityRole() { Name = role.Name });
            }
            else
            {
                var roleDb = _db.Roles.FirstOrDefault(u => u.Id == role.Id);
                if (roleDb == null)
                {
                    return RedirectToAction(nameof(Index));
                }
                roleDb.Name = role.Name;
                roleDb.NormalizedName = role.Name.ToUpper();
                var result = await _roleManager.UpdateAsync(roleDb);

            }
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            var roleDb=_db.Roles.FirstOrDefault(r=>r.Id == id);
            if(roleDb == null)
            {
                return RedirectToAction(nameof(Index));
            }
            var userRoleForThisRole=_db.UserRoles.Where(u=>u.RoleId == id).Count();
            if(userRoleForThisRole > 0)
            {
                return RedirectToAction(nameof(Index));
            }
            await _roleManager.DeleteAsync(roleDb);
            return RedirectToAction(nameof(Index));
        }

    }
}
