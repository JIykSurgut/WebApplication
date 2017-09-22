using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.DataProtection;

namespace WebApp
{
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            app.CreatePerOwinContext((IdentityFactoryOptions<DbContext> options, IOwinContext context) => new DbContext());
            app.CreatePerOwinContext((IdentityFactoryOptions<UserManager<User, int>> options, IOwinContext context) =>
                 new UserManager<User, int>(new UserStore(context.Get<DbContext>()))
                 {
                     UserTokenProvider = new DataProtectorTokenProvider<User, int>(new DpapiDataProtectionProvider("WebApp").Create("Confirmation")),
                     EmailService = new EmailService()
                 });
            app.CreatePerOwinContext((IdentityFactoryOptions<RoleManager<Role, int>> options, IOwinContext context) => new RoleManager<Role, int>(new RoleStore(context.Get<DbContext>())));
            app.CreatePerOwinContext((IdentityFactoryOptions<SignInManager<User, int>> options, IOwinContext context) => new SignInManager<User, int>(context.Get<UserManager<User, int>>(), context.Authentication));

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login")
            });
            
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

        }
    }
}