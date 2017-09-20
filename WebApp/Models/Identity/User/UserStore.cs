using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;


namespace Models
{
    public class AppUserStore :
        IUserStore<AppUser, int>,
        IUserPasswordStore<AppUser, int>,
        IUserLockoutStore<AppUser, int>
    {
        public AppDbContext dbContext
        {
            get;
            private set;
        }

        public AppUserStore(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Dispose()
        {
            if (dbContext != null)
            {
                dbContext.Dispose();
                dbContext = null;
            }
        }

        #region IUserStore :IDisposable
        //
        //Task CreateAsync(TUser user)                 добавить нового пользователя
        //Task DeleteAsync(TUser user)                 удалить пользователя
        //Task<TUser> FindByIdAsync(TKey userId)       найти пользователя по ID
        //Task<TUser> FindByNameAsync(string userName) найти пользователя по имени
        //Task UpdateAsync(TUser user)                 обновить данные пользователя
        public Task CreateAsync(AppUser user)
        {
            IRoleStore <>
            throw new NotImplementedException();
        }
        public Task DeleteAsync(AppUser user)
        {
            throw new NotImplementedException();
        }
        public Task<AppUser> FindByIdAsync(int userId)
        {
            return Task.FromResult<AppUser>(dbContext.UserFindById(userId));
        }
        public Task<AppUser> FindByNameAsync(string userName)
        {
            return Task.FromResult<AppUser>(dbContext.UserFindByName(userName));
        }
        
        public Task UpdateAsync(AppUser user)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region IUserPasswordStore :IUserStore, IDisposable
        //
        //Task<string> GetPasswordHashAsync(TUser user)              получить хеш-пароль пользователя
        //Task<bool> HasPasswordAsync(TUser user)                    есть ли пароль у пользователя
        //Task SetPasswordHashAsync(TUser user, string passwordHash) записать хеш-пароль пользователю
        public Task<string> GetPasswordHashAsync(AppUser user)
        {
            return Task.FromResult<string>(user.PasswordHash);
        }
        public Task<bool> HasPasswordAsync(AppUser user)
        {
            bool result = false;
            if (user.PasswordHash != null) result = true;

            return Task.FromResult<bool>(result);
        }
        public Task SetPasswordHashAsync(AppUser user, string passwordHash)
        {
            user.PasswordHash = passwordHash;
            return Task.FromResult<object>(null);
        }
        #endregion

        #region IUserLockoutStore :IUserStore, IDisposable
        //
        //Task<int> GetAccessFailedCountAsync(TUser user)                   возвращает кол-во неудачных попыток входа
        //Task<bool> GetLockoutEnabledAsync(TUser user)                     может пользователь быть заблокирован?
        //Task<DateTimeOffset> GetLockoutEndDateAsync(TUser user)           получить дату окончания блокировки
        //Task<int> IncrementAccessFailedCountAsync(TUser user)             инкремент неудачных попыток входа
        //Task ResetAccessFailedCountAsync(TUser user)                      сброс неудачных попыток входа
        //Task SetLockoutEnabledAsync(TUser user, bool enabled)             устанавливает блокировку пользователя
        //Task SetLockoutEndDateAsync(TUser user,DateTimeOffset lockoutEnd) задает дату окончания блокировки
        public Task<int> GetAccessFailedCountAsync(AppUser user)
        {
            return Task.FromResult(user.AccessFailedCount);
        }
        public Task<bool> GetLockoutEnabledAsync(AppUser user)
        {
            return Task.FromResult(user.LockoutEnabled);
        }
        public Task<DateTimeOffset> GetLockoutEndDateAsync(AppUser user)
        {
            DateTimeOffset dateTimeOffset = DateTime.SpecifyKind(user.LockoutEndDateUtc, DateTimeKind.Utc);
            return Task.FromResult(dateTimeOffset);
        }
        public Task<int> IncrementAccessFailedCountAsync(AppUser user)
        {
            return Task.FromResult(++user.AccessFailedCount);
        }
        public Task ResetAccessFailedCountAsync(AppUser user)
        {
            user.AccessFailedCount = 0;
            dbContext.UserUpdateById(user);
            return Task.FromResult<object>(null);
        }
        public Task SetLockoutEnabledAsync(AppUser user, bool enabled)
        {
            user.LockoutEnabled = enabled;
            dbContext.UserUpdateById(user);
            return Task.FromResult<object>(null);
        }
        public Task SetLockoutEndDateAsync(AppUser user, DateTimeOffset lockoutEnd)
        {
            user.LockoutEndDateUtc = lockoutEnd.UtcDateTime;
            dbContext.UserUpdateById(user);
            return Task.FromResult<object>(null);
        }
        #endregion
    }
}
