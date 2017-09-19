using Microsoft.AspNet.Identity;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Models
{
    public class AppUser : IUser<int>
    {
        public int Id { get; set; }                        //Идентификатор
        public string PasswordHash { get; set; }           //Хеш-пароль
        public string UserName { get; set; }               //Логин


        public bool LockoutEnabled { get; set; }           //вкл. блокировка для пользователя?
        public int AccessFailedCount { get; set; }         //кол-во неудачных попыток входа
        public DateTime LockoutEndDateUtc { get; set; }    //Дата окончания блокировки



        public AppUser()
        {
            Id = 0;
        }
        public AppUser(string userName)
            : this()
        {
            UserName = userName;
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<AppUser, int> manager)
        {
            return await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
        }
    }
}
