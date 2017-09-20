using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;


namespace Models
{
    public class UserStore :
        IUserStore<User, int>,
        IUserPasswordStore<User, int>,
        IUserLockoutStore<User, int>
    {
        public AppDbContext dbContext
        {
            get;
            private set;
        }

        public UserStore(AppDbContext dbContext) => this.dbContext = dbContext;
        
        public void Dispose()
        {
            if (dbContext != null)
            {
                dbContext.Dispose();
                dbContext = null;
            }
        }

        #region IUserStore
        public Task CreateAsync(User user) => throw new NotImplementedException();       
        public Task DeleteAsync(User user) => throw new NotImplementedException();
        public Task<User> FindByIdAsync(int userId) => Task.FromResult<User>(dbContext.UserFindById(userId));
        public Task<User> FindByNameAsync(string userName) => Task.FromResult<User>(dbContext.UserFindByName(userName));
        public Task UpdateAsync(User user) => throw new NotImplementedException();        
        #endregion

        #region IUserPasswordStore
        public Task<string> GetPasswordHashAsync(User user) => Task.FromResult<string>(user.PasswordHash);       
        public Task<bool> HasPasswordAsync(User user) => Task.FromResult<bool>((user.PasswordHash != null) ? true : false);   
        public Task SetPasswordHashAsync(User user, string passwordHash)
        {
            user.PasswordHash = passwordHash;
            return Task.FromResult<object>(null);
        }
        #endregion

        #region IUserLockoutStore :IUserStore, IDisposable
        public Task<int> GetAccessFailedCountAsync(User user) => Task.FromResult(user.AccessFailedCount);        
        public Task<bool> GetLockoutEnabledAsync(User user) => Task.FromResult(user.LockoutEnabled);     
        public Task<DateTimeOffset> GetLockoutEndDateAsync(User user)
        {
            DateTimeOffset dateTimeOffset = DateTime.SpecifyKind(user.LockoutEndDateUtc, DateTimeKind.Utc);
            return Task.FromResult(dateTimeOffset);
        }
        public Task<int> IncrementAccessFailedCountAsync(User user) => Task.FromResult(++user.AccessFailedCount);
        public Task ResetAccessFailedCountAsync(User user)
        {
            user.AccessFailedCount = 0;
            return Task.FromResult<object>(null);
        }
        public Task SetLockoutEnabledAsync(User user, bool enabled)
        {
            user.LockoutEnabled = enabled;
            return Task.FromResult<object>(null);
        }
        public Task SetLockoutEndDateAsync(User user, DateTimeOffset lockoutEnd)
        {
            user.LockoutEndDateUtc = lockoutEnd.UtcDateTime;
            return Task.FromResult<object>(null);
        }
        #endregion
    }
}
