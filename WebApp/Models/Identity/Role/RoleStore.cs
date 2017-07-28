using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Models
{
    public class AppRoleStore : IRoleStore<AppRole, int>
    {
        public AppDbContext dbContext
        {
            get; private set;
        }
        public IRoleStore<AppRole, int> Roles
        {
            get { throw new NotImplementedException(); }
        }

        public AppRoleStore(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
              
        #region IRoleStore
        //
        // Task CreateAsync(TRole role)                  добавить новую роль
        // Task DeleteAsync(TRole role)                  удалить роль
        // void Dispose()
        // Task<TRole> FindByIdAsync(TKey roleId)        найти роль по ID
        // Task<TRole> FindByNameAsync(string roleName)  найти роль по имени
        // Task UpdateAsync(TRole role)                  обновить роль
        public Task CreateAsync(AppRole role)
        {
            throw new NotImplementedException();
        }
        public Task DeleteAsync(AppRole role)
        {
            throw new NotImplementedException();
        }
        public void Dispose()
        {
            if (dbContext != null)
            {
                dbContext.Dispose();
                dbContext = null;
            }
        }
        public Task<AppRole> FindByIdAsync(int roleId)
        {
            throw new NotImplementedException();
        }
        public Task<AppRole> FindByNameAsync(string roleName)
        {
            throw new NotImplementedException();
        }
        public Task UpdateAsync(AppRole role)
        {
            throw new NotImplementedException();
        }
        #endregion

        public Task<List<AppRole>> RolesGetAll()
        {
            throw new NotImplementedException();
        }
    }
}
