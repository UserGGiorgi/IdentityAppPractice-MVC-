﻿using System.Security.Claims;

namespace IdentityAppPractice.ViewModel
{
    public class UserClaimsViewModel
    {
               public UserClaimsViewModel()
        {
            Claims = new List<UserClaim>();
        }
        public string UserId { get; set; }
        public List<UserClaim> Claims { get; set; }
    }

    public class UserClaim
    {
        public string ClaimType { get; set; }
        public bool IsSelected { get; set; }
    }
}
