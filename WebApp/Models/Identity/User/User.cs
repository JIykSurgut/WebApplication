using Microsoft.AspNet.Identity;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Models
{
    public class User : IUser<int>
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }           
        public bool LockoutEnabled { get; set; }           
        public int AccessFailedCount { get; set; }         
        public DateTime LockoutEndDateUtc { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User, int> manager)
        {
            return await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
        }
    }
}
