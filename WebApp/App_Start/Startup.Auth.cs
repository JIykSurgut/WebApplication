using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using Models;
using Microsoft.AspNet.Identity.Owin;
using System;
using Microsoft.Owin.Security.Google;

namespace WebApp
{
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            app.CreatePerOwinContext((IdentityFactoryOptions<DbContext> options, IOwinContext context) => new DbContext());
            app.CreatePerOwinContext((IdentityFactoryOptions<UserManager> options, IOwinContext context) => new UserManager(new UserStore(context.Get<DbContext>())) {
                UserTokenProvider =
                    new DataProtectorTokenProvider<User, int>(options.DataProtectionProvider.Create("ASP.NET Identity"))
        });
            app.CreatePerOwinContext((IdentityFactoryOptions<RoleManager> options, IOwinContext context) => new RoleManager(new RoleStore(context.Get<DbContext>())));
            app.CreatePerOwinContext((IdentityFactoryOptions<SignInManager> options, IOwinContext context) => new SignInManager(context.Get<UserManager>(), context.Authentication));

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login")
            });
            
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);


            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //   consumerKey: "",
            //   consumerSecret: "");

            //app.UseFacebookAuthentication(
            //   appId: "",
            //   appSecret: "");

            app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            {
                ClientId = "828340761489-7q66eelh2vof4sapa2lelv16a49ga831.apps.googleusercontent.com",
                ClientSecret = "yl4p-Y44Kmo-Z0Rvu-5mDgrs"
            });

        }
    }
}