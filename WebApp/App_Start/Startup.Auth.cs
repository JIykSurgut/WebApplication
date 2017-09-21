using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using Models;
using Microsoft.AspNet.Identity.Owin;


namespace WebApp
{
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            app.CreatePerOwinContext((IdentityFactoryOptions<DbContext> options, IOwinContext context) => new DbContext());
            app.CreatePerOwinContext((IdentityFactoryOptions<UserManager> options, IOwinContext context) => new UserManager(new UserStore(context.Get<DbContext>())));
            app.CreatePerOwinContext((IdentityFactoryOptions<RoleManager<Role, int>> options, IOwinContext context) => new RoleManager<Role, int>( new RoleStore(context.Get<DbContext>())));
            app.CreatePerOwinContext<SignInManager>(SignInManager.Create);

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login")
            });
            
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

        }
    }
}