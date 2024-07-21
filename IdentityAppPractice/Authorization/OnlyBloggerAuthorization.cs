using Microsoft.AspNetCore.Authorization;

namespace IdentityAppPractice.Authorization
{
    public class OnlyBloggerAuthorization : AuthorizationHandler<OnlyBloggerAuthorization>, IAuthorizationRequirement
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OnlyBloggerAuthorization requirement)
        {
            if(context.User.IsInRole("Blogger"))
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }
            return Task.CompletedTask;
        }
    }
}
