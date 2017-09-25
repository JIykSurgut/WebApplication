using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Models
{
    public class RoleStore : IRoleStore<Role, int>
    {
        public DbContext dbContext
        {
            get; private set;
        }

        public RoleStore(DbContext dbContext)
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

        public IRoleStore<Role, int> Roles
        {
            get { throw new NotImplementedException(); }
        }
                      
        #region IRoleStore
        public Task CreateAsync(Role role)
        {
            dbContext.RoleCreate(role);
            return Task.FromResult<object>(null);
        }
        public Task DeleteAsync(Role role)
        {
            dbContext.RoleDelete(role);
            return Task.FromResult<object>(null);
        }      
        public Task<Role> FindByIdAsync(int roleId) => Task.FromResult(dbContext.RoleFindById(roleId));
        public Task<Role> FindByNameAsync(string roleName) => Task.FromResult(dbContext.RoleFindByName(roleName));        
        public Task UpdateAsync(Role role)
        {
            dbContext.RoleUpdate(role);
            return Task.FromResult<object>(null);
        }
        #endregion

        static public Task<List<Role>> RolesGetAll()
        {
            throw new NotImplementedException();
        }
    }
}