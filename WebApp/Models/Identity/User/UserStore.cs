﻿using Microsoft.AspNet.Identity;
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
        public DbContext dbContext
        {
            get;
            private set;
        }

        public UserStore(DbContext dbContext)
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

        #region IUserStore
        public Task CreateAsync(User user)
        {
            dbContext.UserCreate(user);
            return Task.FromResult<object>(null);
        }
        public Task DeleteAsync(User user)
        {
            dbContext.UserDelete(user);
            return Task.FromResult<object>(null);
        }
        public Task<User> FindByIdAsync(int userId) => Task.FromResult(dbContext.UserFindById(userId));
        public Task<User> FindByNameAsync(string userName) => Task.FromResult(dbContext.UserFindByName(userName));
        public Task UpdateAsync(User user)
        {
            dbContext.UserUpdate(user);
            return Task.FromResult<object>(null);
        }
        #endregion

        #region IUserPasswordStore
        public Task<string> GetPasswordHashAsync(User user) => Task.FromResult(user.PasswordHash);
        public Task<bool> HasPasswordAsync(User user) => Task.FromResult(user.PasswordHash != null ? true : false);
        public Task SetPasswordHashAsync(User user, string passwordHash)
        {
            user.PasswordHash = passwordHash;
            return Task.FromResult<object>(null);
        }
        #endregion

        #region IUserLockoutStore
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
