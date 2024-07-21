using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace IdentityAppPractice.Authorization
{
    public class NicknameRequirement:IAuthorizationRequirement
    {
        public NicknameRequirement(string name)
        {
            Name = name;   
        }
        public string Name { get; set; }
    }
}
