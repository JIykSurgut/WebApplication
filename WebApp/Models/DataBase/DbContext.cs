using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading;

namespace Models
{
    public class DbContext : IDisposable
    {
        private SqlConnection connectionString;

        public DbContext(string connectionStringName)
        {        
            connectionString = new SqlConnection(ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString);
        }
        public DbContext()
            : this("DefaultConnection")
        {
        }

        public void Dispose()
        {
            if (connectionString != null)
            {
                connectionString.Dispose();
                connectionString = null;
            }
        }

        public static DbContext Create() => new DbContext();   

        #region DataBase
        private void ConnectionOpen()
        {
            int retries = 3;  //кол-во попыток подключения
            if (connectionString.State == ConnectionState.Open)
            {
                return;
            }
            else
            {
                while (retries >= 0 && connectionString.State != ConnectionState.Open)
                {
                    connectionString.Open();
                    retries--;
                    Thread.Sleep(30); //задержка перед повторной попыткой подключения
                }
            }
        }
        public void ConnectionClose()
        {
            if (connectionString.State == ConnectionState.Open) connectionString.Close();
        }
        private SqlCommand CreateCommand(string commandText, Dictionary<string, object> parameters)
        {
            if (String.IsNullOrEmpty(commandText)) throw new ArgumentException("null or empty commandText. Пустая строка запроса.");

            SqlCommand command = connectionString.CreateCommand();
            command.CommandText = commandText;

            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    var parameter = command.CreateParameter();
                    parameter.ParameterName = param.Key;
                    parameter.Value = param.Value ?? DBNull.Value;
                    command.Parameters.Add(parameter);
                }
            }
            return command;
        }
        public List<Dictionary<string, object>> Query(string commandText, Dictionary<string, object> parameters)
        {
            List<Dictionary<string, object>> rows = null;
            try
            {
                SqlCommand command = CreateCommand(commandText, parameters);
                ConnectionOpen();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    rows = new List<Dictionary<string, object>>();
                    while (reader.Read())
                    {
                        var row = new Dictionary<string, object>();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            var columnName = reader.GetName(i);
                            var columnValue = reader.IsDBNull(i) ? null : reader.GetValue(i);
                            row.Add(columnName, columnValue);
                        }
                        rows.Add(row);
                    }
                }
            }
            finally
            {
                ConnectionClose();
            }
            return rows; //возвращает результат запроса | NULL ошибка запроса
        }
        public SqlParameter[] StoredProcedure(string commandText, SqlParameter[] parameters)
        {
            SqlCommand command = new SqlCommand(commandText, connectionString);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddRange(parameters);
            ConnectionOpen();
            command.ExecuteNonQuery();
            return parameters;
        }
        #endregion

        #region IUserStore
        public void UserCreate(User user)
        {
            StoredProcedure("dbo.UserCreate", UserToParam(user));
        }
        public void UserDelete(User user)
        {
            StoredProcedure("dbo.UserDeleteById", UserToParam(user));
        }
        public User UserFindById(int userId)
        {
            SqlParameter[] parameters = new SqlParameter[] 
            {
                new SqlParameter("id", SqlDbType.Int) { SqlValue = userId, Direction = ParameterDirection.Input }
            };           
            return ParamToUser(StoredProcedure("dbo.UserFindById", parameters));
        }
        public User UserFindByName(string userName)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@UserName",SqlDbType.NVarChar,50) { SqlValue = userName, Direction = ParameterDirection.Input }
            };
            return ParamToUser(StoredProcedure("dbo.UserFindByName", parameters));  
        }
        public void UserUpdate(User user)
        {
            StoredProcedure("dbo.UserFindById", UserToParam(user));
        }
        #region Вспомогательные методы
        private SqlParameter[] UserToParam(User user)
        {
            return new SqlParameter[]
            {
                new SqlParameter("id",SqlDbType.Int) {SqlValue = user.Id, Direction = ParameterDirection.Input},
                new SqlParameter("userName",SqlDbType.NVarChar,50) {SqlValue = user.UserName, Direction = ParameterDirection.Input},
                new SqlParameter("passwordHash",SqlDbType.NVarChar,50) {SqlValue = user.PasswordHash, Direction = ParameterDirection.Input},
                new SqlParameter("lockoutEnabled",SqlDbType.Bit) {SqlValue = user.LockoutEnabled, Direction = ParameterDirection.Input},
                new SqlParameter("accessFailedCount",SqlDbType.Int) {SqlValue = user.AccessFailedCount, Direction = ParameterDirection.Input},
                new SqlParameter("lockoutEndDateUtc",SqlDbType.DateTime) {SqlValue = user.LockoutEndDateUtc, Direction = ParameterDirection.Input}
            };
        } 
        private User ParamToUser(SqlParameter[] parameters)
        {
            return new User()
            {
                Id = Convert.IsDBNull(parameters[0].Value) ? 0 : Convert.ToInt32(parameters[0].Value),
                UserName = Convert.IsDBNull(parameters[1].Value) ? "" : Convert.ToString(parameters[1].Value),
                PasswordHash = Convert.IsDBNull(parameters[2].Value) ? "" : Convert.ToString(parameters[2].Value),
                LockoutEnabled = Convert.IsDBNull(parameters[3].Value) ? false : Convert.ToBoolean(parameters[3].Value),
                AccessFailedCount = Convert.IsDBNull(parameters[4].Value) ? 0 : Convert.ToInt32(parameters[4].Value),
                LockoutEndDateUtc = Convert.IsDBNull(parameters[5].Value) ? new DateTime() : Convert.ToDateTime(parameters[5].Value)
            };
        }
        #endregion
        #endregion

        #region IRoleStore
        public void RoleCreate(Role role)
        {
            //StoredProcedure("dbo.RoleCreate", );
        }
        public void RoleDelete(Role role)
        {
            throw new NotImplementedException();
        }
        public Role RoleFindById(int roleId)
        {
            throw new NotImplementedException();
        }
        public Role RoleFindByName(string roleName)
        {
            throw new NotImplementedException();
        }
        public void RoleUpdate(Role role)
        {
            throw new NotImplementedException();
        }
        #region Вспомогательные методы
        private SqlParameter[] RoleToParam(Role role)
        {
            return new SqlParameter[]
            {
                new SqlParameter("id",SqlDbType.Int) {SqlValue = role.Id, Direction = ParameterDirection.Input},
                new SqlParameter("roleName",SqlDbType.NVarChar,50) {SqlValue = role.Name, Direction = ParameterDirection.Input},
            };
        }
        #endregion
        #endregion

    }
}
