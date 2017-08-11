using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;


namespace Models
{
    public class AppUserStore :
        IUserStore<AppUser, int>,
        IUserRoleStore<AppUser, int>,
        IUserPasswordStore<AppUser, int>,
        IUserTwoFactorStore<AppUser, int>,
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
            throw new NotImplementedException();
        }
        public Task DeleteAsync(AppUser user)
        {
            throw new NotImplementedException();
        }
        public Task<AppUser> FindByIdAsync(int userId)
        {
            throw new NotImplementedException();
        }
        public Task<AppUser> FindByNameAsync(string userName)
        {
            dbContext.UserFindByName(userName);
            //SqlParameter[] parameters = dbContext.UserFindByName(userName);
            //SqlDataReader reader = ((Oracle.ManagedDataAccess.Types.OracleRefCursor)parameters[1].Value).GetDataReader();
            //reader.Read();

            //AppUser user = null;
            //if (reader.HasRows)
            //{
            //    user = new AppUser();
            //    user.Id = Convert.ToInt32(reader["Id"]);
            //    user.UserName = Convert.ToString(reader["UserName"]);
            //    user.PasswordHash = Convert.ToString(reader["PasswordHash"]);
            //    user.SurName = Convert.ToString(reader["SurName"]);
            //    user.FirstName = Convert.ToString(reader["FirstName"]);
            //    user.Patronymic = Convert.ToString(reader["Patronymic"]);
            //    user.LockoutEnabled = (Convert.ToByte(reader["LockoutEnabled"]) == 0) ? false : true;
            //    user.AccessFailedCount = Convert.ToInt32(reader["AccessFailedCount"]);
            //    user.LockoutEndDateUtc = Convert.ToDateTime(reader["LockoutEndDateUTC"]);
            //    user.TwoFactorEnabled = (Convert.ToByte(reader["TwoFactorEnabled"]) == 0) ? false : true;
            //}
            //return Task.FromResult<AppUser>(user);
            throw new NotImplementedException();
        }
        public Task UpdateAsync(AppUser user)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region  IUserRoleStore :IUserStore, IDisposable
        //
        //Task AddToRoleAsync(TUser user, string roleName)      добавить пользователю роль
        //Task<IList<string>> GetRolesAsync(TUser user)         получить роли данного пользователя
        //Task<bool> IsInRoleAsync(TUser user, string roleName) принадлежит роль пользователю?
        //Task RemoveFromRoleAsync(TUser user, string roleName) удалить роль у данного пользователя
        public Task AddToRoleAsync(AppUser user, string roleName)
        {
            throw new NotImplementedException();
        }
        public Task<IList<string>> GetRolesAsync(AppUser user)
        {
            throw new NotImplementedException();
        }
        public Task<bool> IsInRoleAsync(AppUser user, string roleName)
        {
            throw new NotImplementedException();
        }
        public Task RemoveFromRoleAsync(AppUser user, string roleName)
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
            throw new NotImplementedException();
        }
        public Task<bool> HasPasswordAsync(AppUser user)
        {
            throw new NotImplementedException();
        }
        public Task SetPasswordHashAsync(AppUser user, string passwordHash)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }
        public Task<bool> GetLockoutEnabledAsync(AppUser user)
        {
            throw new NotImplementedException();
        }
        public Task<DateTimeOffset> GetLockoutEndDateAsync(AppUser user)
        {
            throw new NotImplementedException();
        }
        public Task<int> IncrementAccessFailedCountAsync(AppUser user)
        {
            throw new NotImplementedException();
        }
        public Task ResetAccessFailedCountAsync(AppUser user)
        {
            throw new NotImplementedException();
        }
        public Task SetLockoutEnabledAsync(AppUser user, bool enabled)
        {
            throw new NotImplementedException();
        }
        public Task SetLockoutEndDateAsync(AppUser user, DateTimeOffset lockoutEnd)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region IUserTwoFactorStore :IUserStore, IDisposable
        //
        //Task<bool> GetTwoFactorEnabledAsync(TUser user) включена двойная аутентификация для пользователя?
        //Task SetTwoFactorEnabledAsync(TUser user, bool enabled) устанавливает двойную аутентификацию 
        public Task SetTwoFactorEnabledAsync(AppUser user, bool enabled)
        {
            throw new NotImplementedException();
        }
        public Task<bool> GetTwoFactorEnabledAsync(AppUser user)
        {
            throw new NotImplementedException();
        }
        #endregion

        public Task<List<AppUser>> UsersGetAllAsync()
        {
            throw new NotImplementedException();
        }
    }
}
