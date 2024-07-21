using IdentityAppPractice.Data;
using IdentityAppPractice.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace IdentityAppPractice.Authorization
{
    public class NicknameAuthorization : AuthorizationHandler<NicknameRequirement>
    {
        public NicknameAuthorization(UserManager<AppUser> userManager,ApplicationDbContext db)
        {
            _userManager = userManager;
            _db = db;
        }

        public UserManager<AppUser> _userManager { get; }
        public ApplicationDbContext _db { get; }

        protected override async Task<Task> HandleRequirementAsync(AuthorizationHandlerContext context, NicknameRequirement requirement)
        {
            string userid=context.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user= _db.AppUser.FirstOrDefault(x => x.Id==userid);
            var claims = Task.Run(async() => await _userManager.GetClaimsAsync(user)).Result;
            var claim=claims.FirstOrDefault(c=>c.Type=="Nickname");
            if(claim != null)
            {
                if(claim.Value.ToLower().Contains(requirement.Name.ToLower()))
                {
                    context.Succeed(requirement);
                    
                }
            }
            return Task.CompletedTask;
        }
    }
}
