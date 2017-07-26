using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;

namespace Models
{
    public class AppRoleManager : RoleManager<AppRole, int>
    {
        public AppRoleManager(AppRoleStore appRoleStore)
            : base(appRoleStore)
        {
            this.appRoleStore = appRoleStore;
        }

        public AppRoleStore appRoleStore
        {
            get;
            protected set;
        }

        public Task<List<AppRole>> GetRolesAsync()
        {
            return appRoleStore.RolesGetAll();
        }

        public static AppRoleManager Create(IdentityFactoryOptions<AppRoleManager> options, IOwinContext context)
        {
            return new AppRoleManager(new AppRoleStore(context.Get<AppDbContext>()));
        }
    }
}