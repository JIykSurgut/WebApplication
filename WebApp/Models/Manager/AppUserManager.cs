using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System;

namespace Models
{
    public class AppUserManager : UserManager<AppUser, int>
    {
        public AppUserManager(AppUserStore appUserStore)
            : base(appUserStore)
        {
            this.appUserStore = appUserStore;
        }

        public AppUserStore appUserStore
        {
            get;
            protected set;
        }

        public Task<List<AppUser>> GetUsersAsync()
        {
            return appUserStore.UsersGetAllAsync();
        }

        public static AppUserManager Create(IdentityFactoryOptions<AppUserManager> options, IOwinContext context)
        {
            return new AppUserManager(new AppUserStore(context.Get<AppDbContext>()));
        }
    }
}