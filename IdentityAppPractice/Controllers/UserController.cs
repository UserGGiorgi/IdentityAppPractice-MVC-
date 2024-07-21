using IdentityAppPractice.Data;
using IdentityAppPractice.Models;
using IdentityAppPractice.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static IdentityAppPractice.ViewModel.UserClaimsViewModel;



namespace IdentityAppPractice.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<AppUser> _userManager;

        public UserController(ApplicationDbContext db,UserManager<AppUser> user)
        {
            _db=db;
            _userManager=user;

        }
        public IActionResult Index()
        {
            var userList=_db.AppUser.ToList();
            var roleList=_db.UserRoles.ToList();
            var roles=_db.Roles.ToList();

            foreach (var user in userList)
            {
                var role =roleList.FirstOrDefault(x=>x.UserId==user.Id);
                if (role == null)
                {
                    user.Role = "None";
                }
                else
                {
                    user.Role = roles.FirstOrDefault(u => u.Id == role.RoleId).Name;
                }
                
            }
            return View(userList);
        }
        [HttpGet]
        public IActionResult Edit(string userId)
        {
            var user=_db.AppUser.FirstOrDefault(u=>u.Id==userId);
            if(user == null)
            {
                return NotFound();
            }
            var userRole = _db.UserRoles.ToList();
            var roles= _db.Roles.ToList();
            var role = userRole.FirstOrDefault(u => u.UserId==user.Id);
            if (role != null)
            {
                user.RoleId = roles.FirstOrDefault(u => u.Id == role.RoleId).Id;
            }
                user.RoleList = _db.Roles.Select(u => new SelectListItem
                {
                    Text=u.Name,
                    Value=u.Id
                });
                return View(user);             
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>  Edit(AppUser user)
        {
            if(ModelState.IsValid)
            {              
                var UserDbValue=await _db.AppUser.FirstOrDefaultAsync(u=> u.Id== user.Id);
                if (UserDbValue == null)
                {
                    return NotFound();
                }
                var userRole = _db.UserRoles.FirstOrDefault(u => u.RoleId == UserDbValue.Id);
                if (userRole != null)
                {
                    var previousRoleName = _db.Roles.Where(u=>u.Id==userRole.RoleId).Select(e=>e.Name).FirstOrDefault();
                    await _userManager.RemoveFromRoleAsync(UserDbValue,previousRoleName);
                }
                await _userManager.AddToRoleAsync(UserDbValue,_db.Roles.FirstOrDefault(u=>u.Id==user.RoleId).Name);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
                
            }
            user.RoleList = _db.Roles.Select(u => new SelectListItem
            {
                Text=u.Name,
                Value=u.Id

            });
           return View(user);
        }
        [HttpPost]
        public IActionResult Delete(string UserId)
        {
            var user = _db.AppUser.FirstOrDefault(u=>u.Id==UserId);
            if(user == null)
            {
                return NotFound();
            }
            _db.AppUser.Remove(user);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> ManageClaims(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            var existingUserClaims = await _userManager.GetClaimsAsync(user);

            var model = new UserClaimsViewModel()
            {
                UserId = userId
            };

            foreach (Claim claim in ClaimStore.claimList)
            {
                UserClaim userClaim = new UserClaim
                {
                    ClaimType = claim.Type
                };
                if (existingUserClaims.Any(c => c.Type == claim.Type))
                {
                    userClaim.IsSelected = true;
                }
                model.Claims.Add(userClaim);
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManageClaims(UserClaimsViewModel userClaimsViewModel)
        {
            var user = await _userManager.FindByIdAsync(userClaimsViewModel.UserId);

            if (user == null)
            {
                return NotFound();
            }

            var claims = await _userManager.GetClaimsAsync(user);
            var result = await _userManager.RemoveClaimsAsync(user, claims);

            if (!result.Succeeded)
            {
                return View(userClaimsViewModel);
            }

            result = await _userManager.AddClaimsAsync(user,
                userClaimsViewModel.Claims.Where(c => c.IsSelected).Select(c => new Claim(c.ClaimType, c.IsSelected.ToString())));

            if (!result.Succeeded)
            {
                return View(userClaimsViewModel);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
