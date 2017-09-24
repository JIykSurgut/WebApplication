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
        public string SecurityStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }

        public User()
        {
            Id = 0;
            UserName = "";
            PasswordHash = "";
            LockoutEnabled = false;
            AccessFailedCount = 0;
            LockoutEndDateUtc = new DateTime(2000, 1, 1);
            TwoFactorEnabled = false;
            Email = "";
            EmailConfirmed = false;
            SecurityStamp = "";
            PhoneNumber = "";
            PhoneNumberConfirmed = false;
        }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User, int> userManager)
        {
            ClaimsIdentity claimsIdentity =  await userManager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            claimsIdentity.AddClaim(new Claim("mail", Email));
            return claimsIdentity;
        }
    }
}
