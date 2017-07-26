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
            dbContext.RoleInsert(role);
            return Task.FromResult<object>(null);
        }
        public Task DeleteAsync(AppRole role)
        {
            dbContext.RoleDeleteById(role.Id);
            return Task.FromResult<object>(null);
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
            OracleParameter[] parameters = dbContext.RoleFindById(roleId);
            OracleDataReader reader = ((Oracle.ManagedDataAccess.Types.OracleRefCursor)parameters[1].Value).GetDataReader();
            reader.Read();

            AppRole role = null;
            if (reader.HasRows)
            {
                role = new AppRole();
                role.Id = Convert.ToInt32(reader["Id"]);
                role.Name = Convert.ToString(reader["Name"]);
            }
            return Task.FromResult<AppRole>(role);
        }
        public Task<AppRole> FindByNameAsync(string roleName)
        {
            OracleParameter[] parameters = dbContext.RoleFindByName(roleName);
            OracleDataReader reader = ((Oracle.ManagedDataAccess.Types.OracleRefCursor)parameters[1].Value).GetDataReader();
            reader.Read();

            AppRole role = null;
            if (reader.HasRows)
            {
                role = new AppRole();
                role.Id = Convert.ToInt32(reader["Id"]);
                role.Name = Convert.ToString(reader["Name"]);
            }
            return Task.FromResult<AppRole>(role);
        }
        public Task UpdateAsync(AppRole role)
        {
            dbContext.RoleUpdateById(role);
            return Task.FromResult<object>(null);
        }
        #endregion

        public Task<List<AppRole>> RolesGetAll()
        {
            OracleParameter[] parameters = dbContext.RolesGetAll();
            OracleDataReader reader = ((Oracle.ManagedDataAccess.Types.OracleRefCursor)parameters[0].Value).GetDataReader();

            List<AppRole> roles = new List<AppRole>();
            while (reader.Read())
            {
                AppRole role = new AppRole();
                role.Id = Convert.ToInt32(reader["Id"]);
                role.Name = Convert.ToString(reader["Name"]);
                roles.Add(role);
            }
            return Task.FromResult<List<AppRole>>(roles);
        }
    }
}
