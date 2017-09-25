using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;

namespace Models
{
    public class RoleManager : RoleManager<Role, int>
    {
        public RoleManager(RoleStore roleStore)
            : base(roleStore)
        {
            #region base .ctor
            //if (store == null)
            //{
            //    throw new ArgumentNullException("store");
            //}
            //this.Store = store;
            //this.RoleValidator = new RoleValidator<TRole, TKey>(this);
            #endregion
        }

        //public Task<List<Role>> GetRolesAsync()
        //{            
        //    return RoleStore.RolesGetAll();
        //}
    }
}