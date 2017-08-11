using Microsoft.AspNet.Identity;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Models
{
    public class AppUser : IUser<int>
    {
        public int Id { get; set; }                        //Идентификатор
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }           //Хеш-пароль
        public string SecurityStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set;}
        public bool TwoFactorEnabled { get; set; }         //вкл. двойная аутентификация?
        public DateTime LockoutEndDateUtc { get; set; }    //Дата окончания блокировки
        public bool LockoutEnabled { get; set; }           //вкл. блокировка для пользователя?
        public int AccessFailedCount { get; set; }         //кол-во неудачных попыток входа
        public string UserName { get; set; }               //Логин
        
        //public string SurName { get; set; }                //Фамилия
        //public string FirstName { get; set; }              //Имя     
        //public string Patronymic { get; set; }             //Отчество
        
        
       

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
