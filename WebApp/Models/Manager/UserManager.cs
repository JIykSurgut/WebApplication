using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.DataProtection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Models
{
    public class UserManager : UserManager<User, int>
    {
        public UserManager(UserStore userStore)
            : base(userStore) 
        {
            #region base .ctor
            //if (store == null)
            //{
            //    throw new ArgumentNullException("store");
            //}
            //this.Store = store;
            //this.UserValidator = new UserValidator<TUser, TKey>(this);
            //this.PasswordValidator = new MinimumLengthValidator(6);
            //this.PasswordHasher = new PasswordHasher();
            //this.ClaimsIdentityFactory = new ClaimsIdentityFactory<TUser, TKey>();
            #endregion

            //UserTokenProvider = new DataProtectorTokenProvider<User, int>(new DpapiDataProtectionProvider().Create("Confirmation"));
            EmailService = new EmailService();
            SmsService = new SmsService();
            
            //Значения по умолчанию
            //1.Указывает включать блокировку при создании пользователей
            UserLockoutEnabledByDefault = false;
            //2.Время на которое блокируется пользователь
            DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            //3.Макс. кол-во неверных попыток входа
            MaxFailedAccessAttemptsBeforeLockout = 5;
            //4.Задает метод хеширования паролей
            //PasswordHasher
            //5.Проверка паролей перед сохранением
            PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6, //мин. длина пароля
                RequireNonLetterOrDigit = false, //спец символы
                RequireDigit = false,  //0 - 9 цифры 
                RequireLowercase = false, //строчные буквы
                RequireUppercase = false, //заглавные буквы
            };
            //6.Проверка пользователя перед сохранением
            //UserValidator

        }

        //public Task<List<User>> GetUsersAsync()
        //{
        //    //return appUserStore.UsersGetAllAsync();
        //    return null;
        //}
    }
}