using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System;

namespace Models
{
    public class UserManager : UserManager<AppUser, int>
    {
        public UserManager(AppUserStore appUserStore)
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
            //return appUserStore.UsersGetAllAsync();
            return null;
        }

        public static AppUserManager Create(IdentityFactoryOptions<AppUserManager> options, IOwinContext context)
        {
            return new AppUserManager(new AppUserStore(context.Get<AppDbContext>()));
        }
    }
}