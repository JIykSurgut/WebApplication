using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

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
            dbContext.UserInsert(user);
            return Task.FromResult<object>(null);
        }
        public Task DeleteAsync(AppUser user)
        {
            dbContext.UserDeleteById(user.Id);
            return Task.FromResult<Object>(null);
        }
        public Task<AppUser> FindByIdAsync(int userId)
        {
            OracleParameter[] parameters = dbContext.UserFindById(userId);
            OracleDataReader reader = ((Oracle.ManagedDataAccess.Types.OracleRefCursor)parameters[1].Value).GetDataReader();
            reader.Read();

            AppUser user = null;
            if (reader.HasRows)
            {
                user = new AppUser();
                user.Id = Convert.ToInt32(reader["Id"]);
                user.UserName = Convert.ToString(reader["UserName"]);
                user.PasswordHash = Convert.ToString(reader["PasswordHash"]);
                user.SurName = Convert.ToString(reader["SurName"]);
                user.FirstName = Convert.ToString(reader["FirstName"]);
                user.Patronymic = Convert.ToString(reader["Patronymic"]);
                user.LockoutEnabled = (Convert.ToByte(reader["LockoutEnabled"]) == 0) ? false : true;
                user.AccessFailedCount = Convert.ToInt32(reader["AccessFailedCount"]);
                user.LockoutEndDateUtc = Convert.ToDateTime(reader["LockoutEndDateUTC"]);
                user.TwoFactorEnabled = (Convert.ToByte(reader["TwoFactorEnabled"]) == 0) ? false : true;
            }
            return Task.FromResult<AppUser>(user);
        }
        public Task<AppUser> FindByNameAsync(string userName)
        {
            OracleParameter[] parameters = dbContext.UserFindByName(userName);
            OracleDataReader reader = ((Oracle.ManagedDataAccess.Types.OracleRefCursor)parameters[1].Value).GetDataReader();
            reader.Read();

            AppUser user = null;
            if (reader.HasRows)
            {
                user = new AppUser();
                user.Id = Convert.ToInt32(reader["Id"]);
                user.UserName = Convert.ToString(reader["UserName"]);
                user.PasswordHash = Convert.ToString(reader["PasswordHash"]);
                user.SurName = Convert.ToString(reader["SurName"]);
                user.FirstName = Convert.ToString(reader["FirstName"]);
                user.Patronymic = Convert.ToString(reader["Patronymic"]);
                user.LockoutEnabled = (Convert.ToByte(reader["LockoutEnabled"]) == 0) ? false : true;
                user.AccessFailedCount = Convert.ToInt32(reader["AccessFailedCount"]);
                user.LockoutEndDateUtc = Convert.ToDateTime(reader["LockoutEndDateUTC"]);
                user.TwoFactorEnabled = (Convert.ToByte(reader["TwoFactorEnabled"]) == 0) ? false : true;
            }
            return Task.FromResult<AppUser>(user);
        }
        public Task UpdateAsync(AppUser user)
        {
            dbContext.UserUpdateById(user);
            return Task.FromResult<Object>(null);
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
            dbContext.UserRoleInsert(user.Id, roleName);
            return Task.FromResult<object>(null);
        }
        public Task<IList<string>> GetRolesAsync(AppUser user)
        {
            OracleParameter[] parameters = dbContext.RolesGetByUserId(user.Id);
            OracleDataReader reader = ((Oracle.ManagedDataAccess.Types.OracleRefCursor)parameters[1].Value).GetDataReader();

            IList<string> roleNames = new List<string>();
            while (reader.Read())
            {
                roleNames.Add(Convert.ToString(reader["Name"]));
            }
            return Task.FromResult<IList<string>>(roleNames);
        }
        public Task<bool> IsInRoleAsync(AppUser user, string roleName)
        {
            OracleParameter[] parameters = dbContext.IsRoleInUserd(user.Id, roleName);
            int result = int.Parse(parameters[2].Value.ToString());
            if (result != 0)
            {
                return Task.FromResult<bool>(true);
            }
            return Task.FromResult<bool>(false);
        }
        public Task RemoveFromRoleAsync(AppUser user, string roleName)
        {
            dbContext.RoleDeleteInUser(user.Id, roleName);
            return Task.FromResult<object>(null);
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

        #region IUserTwoFactorStore :IUserStore, IDisposable
        //
        //Task<bool> GetTwoFactorEnabledAsync(TUser user) включена двойная аутентификация для пользователя?
        //Task SetTwoFactorEnabledAsync(TUser user, bool enabled) устанавливает двойную аутентификацию 
        public Task SetTwoFactorEnabledAsync(AppUser user, bool enabled)
        {
            user.TwoFactorEnabled = enabled;
            dbContext.UserUpdateById(user);
            return Task.FromResult<object>(null);
        }
        public Task<bool> GetTwoFactorEnabledAsync(AppUser user)
        {
            return Task.FromResult(user.TwoFactorEnabled);
        }
        #endregion

        public Task<List<AppUser>> UsersGetAllAsync()
        {
            OracleParameter[] parameters = dbContext.UsersGetAll();
            OracleDataReader reader = ((Oracle.ManagedDataAccess.Types.OracleRefCursor)parameters[0].Value).GetDataReader();

            List<AppUser> users = new List<AppUser>();
            while (reader.Read())
            {
                AppUser user = new AppUser();
                user.Id = Convert.ToInt32(reader["Id"]);
                user.UserName = Convert.ToString(reader["UserName"]);
                user.PasswordHash = Convert.ToString(reader["PasswordHash"]);
                user.SurName = Convert.ToString(reader["SurName"]);
                user.FirstName = Convert.ToString(reader["FirstName"]);
                user.Patronymic = Convert.ToString(reader["Patronymic"]);
                user.LockoutEnabled = (Convert.ToByte(reader["LockoutEnabled"]) == 0) ? false : true;
                user.AccessFailedCount = Convert.ToInt32(reader["AccessFailedCount"]);
                user.LockoutEndDateUtc = Convert.ToDateTime(reader["LockoutEndDateUTC"]);
                user.TwoFactorEnabled = (Convert.ToByte(reader["TwoFactorEnabled"]) == 0) ? false : true;
                users.Add(user);
            }
            return Task.FromResult<List<AppUser>>(users);
        }
    }
}
