using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Models
{
    //public class UserManager : UserManager<User, int>
    //{
    //    public UserManager(UserStore userStore)
    //        : base(userStore)
    //    {
    //        this.UserStore = userStore;
    //    }

    //    public UserStore UserStore
    //    {
    //        get;
    //        protected set;
    //    }

    //    public Task<List<User>> GetUsersAsync()
    //    {
    //        //return appUserStore.UsersGetAllAsync();
    //        return null;
    //    }

    //    public static UserManager Create(IdentityFactoryOptions<UserManager> options, IOwinContext context)
    //    {
    //        return new UserManager(new UserStore(context.Get<DbContext>()));
    //    }
    //}
}