using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;

namespace Models
{
    //public class RoleManager : RoleManager<Role, int>
    //{
    //    public RoleManager(RoleStore appRoleStore)
    //        : base(appRoleStore)
    //    {
    //        this.appRoleStore = appRoleStore;
    //    }

    //    public RoleStore appRoleStore
    //    {
    //        get;
    //        protected set;
    //    }

    //    public Task<List<Role>> GetRolesAsync()
    //    {
    //        return RoleStore.RolesGetAll();
    //    }

    //    public static RoleManager Create(IdentityFactoryOptions<RoleManager> options, IOwinContext context)
    //    {
    //        return new RoleManager(new RoleStore(context.Get<DbContext>()));
    //    }
    //}
}