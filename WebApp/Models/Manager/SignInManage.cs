using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Models
{
    public class SignInManager : SignInManager<User, int>
    {
        public SignInManager(UserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
            #region base .ctor
            //if (userManager == null)
            //{
            //    throw new ArgumentNullException("userManager");
            //}
            //if (authenticationManager == null)
            //{
            //    throw new ArgumentNullException("authenticationManager");
            //}
            //this.UserManager = userManager;
            //this.AuthenticationManager = authenticationManager;
            #endregion
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(User user)
        {
            return user.GenerateUserIdentityAsync(UserManager);
        }
    }
}