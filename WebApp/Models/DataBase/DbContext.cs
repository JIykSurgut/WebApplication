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
        public SqlDataReader StoredProcedure(string commandText)
        {
            SqlCommand command = new SqlCommand(commandText, connectionString);
            command.CommandType = CommandType.StoredProcedure;
            ConnectionOpen();
            return command.ExecuteReader();
        }
        #endregion

        #region IUserStore
        public void UserCreate(User user) => StoredProcedure("dbo.UserCreate", UserToParam(user));
        public void UserDelete(User user) => StoredProcedure("dbo.UserDeleteById", UserToParam(user));
        public User UserFindById(int userId) => ParamToUser(StoredProcedure("dbo.UserFindById", UserToParam(new User() { Id = userId })));        
        public User UserFindByName(string userName) => ParamToUser(StoredProcedure("dbo.UserFindByName", UserToParam(new User() { UserName = userName })));         
        public void UserUpdate(User user) => StoredProcedure("dbo.UserUpdate", UserToParam(user));
        #endregion
        #region Вспомогательные методы
        private SqlParameter[] UserToParam(User user) => new SqlParameter[]
            {
                new SqlParameter("@id",SqlDbType.Int) {SqlValue = user.Id, Direction = ParameterDirection.InputOutput},
                new SqlParameter("@userName",SqlDbType.NVarChar,50) {SqlValue = user.UserName, Direction = ParameterDirection.InputOutput},
                new SqlParameter("@passwordHash",SqlDbType.NVarChar,512) {SqlValue = user.PasswordHash, Direction = ParameterDirection.InputOutput},
                new SqlParameter("@lockoutEnabled",SqlDbType.Bit) {SqlValue = user.LockoutEnabled, Direction = ParameterDirection.InputOutput},
                new SqlParameter("@accessFailedCount",SqlDbType.Int) {SqlValue = user.AccessFailedCount, Direction = ParameterDirection.InputOutput},
                new SqlParameter("@lockoutEndDateUtc",SqlDbType.DateTime) {SqlValue = user.LockoutEndDateUtc, Direction = ParameterDirection.InputOutput},
                new SqlParameter("@twoFactorEnabled",SqlDbType.Bit) {SqlValue = user.TwoFactorEnabled, Direction = ParameterDirection.InputOutput},
                new SqlParameter("@email",SqlDbType.NVarChar,50) {SqlValue = user.Email, Direction = ParameterDirection.InputOutput},
                new SqlParameter("@emailConfirmed",SqlDbType.Bit) {SqlValue = user.EmailConfirmed, Direction = ParameterDirection.InputOutput},
                new SqlParameter("@securityStamp",SqlDbType.NVarChar,256) {SqlValue = user.SecurityStamp, Direction = ParameterDirection.InputOutput},
                new SqlParameter("@phoneNumber",SqlDbType.NVarChar,256) {SqlValue = user.PhoneNumber, Direction = ParameterDirection.InputOutput},
                new SqlParameter("@phoneNumberConfirmed",SqlDbType.Bit) {SqlValue = user.PhoneNumberConfirmed, Direction = ParameterDirection.InputOutput},
            }; 
        private User ParamToUser(SqlParameter[] parameters) => new User()
            {
                Id = Convert.ToInt32(parameters[0].Value),
                UserName = Convert.ToString(parameters[1].Value),
                PasswordHash = Convert.ToString(parameters[2].Value),
                LockoutEnabled = Convert.ToBoolean(parameters[3].Value),
                AccessFailedCount = Convert.ToInt32(parameters[4].Value),
                LockoutEndDateUtc = Convert.ToDateTime(parameters[5].Value),
                TwoFactorEnabled = Convert.ToBoolean(parameters[6].Value),
                Email =  Convert.ToString(parameters[7].Value),
                EmailConfirmed = Convert.ToBoolean(parameters[8].Value),
                SecurityStamp = Convert.ToString(parameters[9].Value),
                PhoneNumber = Convert.ToString(parameters[10].Value),
                PhoneNumberConfirmed = Convert.ToBoolean(parameters[11].Value)
            };
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

        #region IUserEmailStore
        public User FindByEmail(string email) => ParamToUser(StoredProcedure("dbo.UserFindByEmail", UserToParam(new User() { Email = email })));
        #endregion

        #region JStree
        public SqlDataReader GetJStree()
        {
            return StoredProcedure("dbo.GetNodeJStree");
            
        }

        public SqlDataReader GetTreeSolush()
        {
            return StoredProcedure("dbo.GetTreeSolush");
        }

        public SqlParameter[] GetArticle(int id)
        {
            return  StoredProcedure("dbo.GetArticle", new[]
            {
                new SqlParameter("@id",SqlDbType.Int) {SqlValue = id, Direction = ParameterDirection.Input },
                new SqlParameter("@html", SqlDbType.NVarChar,int.MaxValue) {Direction =ParameterDirection.Output }
            });
        }
        #endregion
    }
}
