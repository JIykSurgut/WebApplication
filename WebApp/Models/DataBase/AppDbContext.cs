using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading;

namespace Models
{
    public class AppDbContext : IDisposable
    {
        private SqlConnection connectionString;

        public AppDbContext(string connectionStringName)
        {
            string connectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
            this.connectionString = new SqlConnection(connectionString);
        }
        public AppDbContext()
            : this("DefaultConnection")
        {
        }

        public static AppDbContext Create()
        {
            return new AppDbContext();
        }

        //Описание методов
        //void ConnectionOpen()                       открыть соединение
        //void ConnectionClose()                      закрыть соединение
        //OracleCommand CreateCommand(...)            создание OracleCommand
        //List<Dictionary<string, string>> Query(...) запрос
        //int NonQuery(...)                           запрос NonQuery
        //object QueryValue(...)                      запрос возвращает одно значение
        //string QueryStrValue(...)                   запрос возвращает одно значение строку      
        //void Dispose()
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

        //
        public SqlParameter[] StoredProcedure(string commandText, SqlParameter[] parameters)
        {
            SqlCommand command = new SqlCommand(commandText, connectionString);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddRange(parameters);
            ConnectionOpen();
            command.ExecuteNonQuery();

            return parameters;
        }
        //
        public void Dispose()
        {
            if (connectionString != null)
            {
                connectionString.Dispose();
                connectionString = null;
            }
        }

        //методы
        //IUserStore
        public SqlParameter[] UserInsert(AppUser user)
        {
            string commandText = "dbo.UserInsert";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("id",SqlDbType.Int) {SqlValue = user.Id, Direction = ParameterDirection.Input},
                new SqlParameter("email",SqlDbType.NVarChar, 50) {SqlValue = user.Email, Direction = ParameterDirection.Input},
                new SqlParameter("emailConfirmed",SqlDbType.Bit) {SqlValue = user.EmailConfirmed, Direction = ParameterDirection.Input},
                new SqlParameter("passwordHash", SqlDbType.NVarChar,50) {SqlValue = user.PasswordHash, Direction = ParameterDirection.Input},
                new SqlParameter("securityStamp", SqlDbType.NVarChar,50) {SqlValue = user.SecurityStamp, Direction = ParameterDirection.Input},
                new SqlParameter("phoneNumber",SqlDbType.NVarChar,50) {SqlValue = user.PhoneNumber, Direction = ParameterDirection.Input},
                new SqlParameter("phoneNumberConfirmed", SqlDbType.Bit) {SqlValue = user.PhoneNumberConfirmed, Direction = ParameterDirection.Input},
                new SqlParameter("twoFactorEnabled", SqlDbType.Bit) {SqlValue = user.TwoFactorEnabled, Direction = ParameterDirection.Input},
                new SqlParameter("lockoutEndDateUtc", SqlDbType.DateTime) {SqlValue = user.LockoutEndDateUtc, Direction = ParameterDirection.Input},
                new SqlParameter("lockoutEnabled", SqlDbType.Bit) {SqlValue = user.LockoutEnabled, Direction = ParameterDirection.Input},
                new SqlParameter("accessFailedCount", SqlDbType.Int) {SqlValue = user.AccessFailedCount, Direction = ParameterDirection.Input},
                new SqlParameter("userName",SqlDbType.NVarChar,50) {SqlValue = user.UserName, Direction = ParameterDirection.Input}
            };
            return StoredProcedure(commandText, parameters);
        }
        public SqlParameter[] UserDeleteById(int userId)
        {
            string commandText = "dbo.UserDeleteById";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("id",SqlDbType.Int) {SqlValue = userId, Direction = ParameterDirection.Input}
            };
            return StoredProcedure(commandText, parameters);
        }
        public AppUser UserFindById(int userId)
        {
            string commandText = "dbo.UserFindById";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("id", SqlDbType.Int) {SqlValue = userId, Direction = ParameterDirection.Input},

                new SqlParameter("id",SqlDbType.Int)                        {Direction = ParameterDirection.Output},
                new SqlParameter("Email",SqlDbType.NVarChar, 50)            {Direction = ParameterDirection.Output},
                new SqlParameter("EmailConfirmed",SqlDbType.Bit)            {Direction = ParameterDirection.Output},
                new SqlParameter("PasswordHash", SqlDbType.NVarChar,50)     {Direction = ParameterDirection.Output},
                new SqlParameter("SecurityStamp", SqlDbType.NVarChar,50)    {Direction = ParameterDirection.Output},
                new SqlParameter("phoneNumber",SqlDbType.NVarChar,50)       {Direction = ParameterDirection.Output},
                new SqlParameter("PhoneNumberConfirmed", SqlDbType.Bit)     {Direction = ParameterDirection.Output},
                new SqlParameter("TwoFactorEnabled", SqlDbType.Bit)         {Direction = ParameterDirection.Output},
                new SqlParameter("LockoutEndDateUtc", SqlDbType.DateTime)   {Direction = ParameterDirection.Output},
                new SqlParameter("LockoutEnabled", SqlDbType.Bit)           {Direction = ParameterDirection.Output},
                new SqlParameter("AccessFailedCount", SqlDbType.Int)        {Direction = ParameterDirection.Output},
                new SqlParameter("UserName",SqlDbType.NVarChar,50)          {Direction = ParameterDirection.Output},
            };
            parameters = StoredProcedure(commandText, parameters);
            return new AppUser()
            {
                Id = Convert.IsDBNull(parameters[1].Value) ? 0 : Convert.ToInt32(parameters[1].Value),
                Email = Convert.IsDBNull(parameters[2].Value) ? "" : Convert.ToString(parameters[2].Value),
                EmailConfirmed = Convert.IsDBNull(parameters[3].Value) ? false : Convert.ToBoolean(parameters[3].Value),
                PasswordHash = Convert.IsDBNull(parameters[4].Value) ? "" : Convert.ToString(parameters[4].Value),
                SecurityStamp = Convert.IsDBNull(parameters[5].Value) ? "" : Convert.ToString(parameters[5].Value),
                PhoneNumber = Convert.IsDBNull(parameters[6].Value) ? "" : Convert.ToString(parameters[6].Value),
                PhoneNumberConfirmed = Convert.IsDBNull(parameters[7].Value) ? false : Convert.ToBoolean(parameters[7].Value),
                TwoFactorEnabled = Convert.IsDBNull(parameters[8].Value) ? false : Convert.ToBoolean(parameters[8].Value),
                LockoutEndDateUtc = Convert.IsDBNull(parameters[9].Value) ? new DateTime(0) : Convert.ToDateTime(parameters[9].Value),
                AccessFailedCount = Convert.IsDBNull(parameters[10].Value) ? 0 : Convert.ToInt32(parameters[10].Value),
                UserName = Convert.IsDBNull(parameters[11].Value) ? "" : Convert.ToString(parameters[11].Value)
            };
        }
        public AppUser UserFindByName(string userName)
        {
            string commandText = "dbo.UserFindByName";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("userName", SqlDbType.NVarChar, 50) {SqlValue = userName, Direction = ParameterDirection.Input},

                new SqlParameter("id",SqlDbType.Int)                        {Direction = ParameterDirection.Output},
                new SqlParameter("Email",SqlDbType.NVarChar, 50)            {Direction = ParameterDirection.Output},
                new SqlParameter("EmailConfirmed",SqlDbType.Bit)            {Direction = ParameterDirection.Output},
                new SqlParameter("PasswordHash", SqlDbType.NVarChar,50)     {Direction = ParameterDirection.Output},
                new SqlParameter("SecurityStamp", SqlDbType.NVarChar,50)    {Direction = ParameterDirection.Output},                
                new SqlParameter("phoneNumber",SqlDbType.NVarChar,50)       {Direction = ParameterDirection.Output},
                new SqlParameter("PhoneNumberConfirmed", SqlDbType.Bit)     {Direction = ParameterDirection.Output},
                new SqlParameter("TwoFactorEnabled", SqlDbType.Bit)         {Direction = ParameterDirection.Output},
                new SqlParameter("LockoutEndDateUtc", SqlDbType.DateTime)   {Direction = ParameterDirection.Output},
                new SqlParameter("LockoutEnabled", SqlDbType.Bit)           {Direction = ParameterDirection.Output},
                new SqlParameter("AccessFailedCount", SqlDbType.Int)        {Direction = ParameterDirection.Output},
                new SqlParameter("UserName",SqlDbType.NVarChar,50)          {Direction = ParameterDirection.Output},
            };
            parameters = StoredProcedure(commandText, parameters);
            return new AppUser()
            {
                Id =                    Convert.IsDBNull(parameters[1].Value) ? 0       : Convert.ToInt32(parameters[1].Value),
                Email =                 Convert.IsDBNull(parameters[2].Value) ? ""      : Convert.ToString(parameters[2].Value),
                EmailConfirmed =        Convert.IsDBNull(parameters[3].Value) ? false   : Convert.ToBoolean(parameters[3].Value),
                PasswordHash =          Convert.IsDBNull(parameters[4].Value) ? ""      : Convert.ToString(parameters[4].Value),
                SecurityStamp =         Convert.IsDBNull(parameters[5].Value) ? ""      : Convert.ToString(parameters[5].Value),
                PhoneNumber =           Convert.IsDBNull(parameters[6].Value) ? ""      : Convert.ToString(parameters[6].Value),
                PhoneNumberConfirmed =  Convert.IsDBNull(parameters[7].Value) ? false   : Convert.ToBoolean(parameters[7].Value),
                TwoFactorEnabled =      Convert.IsDBNull(parameters[8].Value) ? false   : Convert.ToBoolean(parameters[8].Value),
                LockoutEndDateUtc =     Convert.IsDBNull(parameters[9].Value) ? new DateTime(0) : Convert.ToDateTime(parameters[9].Value),
                AccessFailedCount =     Convert.IsDBNull(parameters[10].Value) ? 0      : Convert.ToInt32(parameters[10].Value),
                UserName =              Convert.IsDBNull(parameters[11].Value) ? ""     : Convert.ToString(parameters[11].Value)
            };  
        }
        public SqlParameter[] UserUpdateById(AppUser user)
        {
            string commandText = "dbo.UserUpdateById";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("id",SqlDbType.Int) {SqlValue = user.Id, Direction = ParameterDirection.Input},
                new SqlParameter("email",SqlDbType.NVarChar,50) { Direction = ParameterDirection.Input },
                new SqlParameter("emailConfirmed",SqlDbType.Bit) { Direction = ParameterDirection.Input },
                new SqlParameter("passwordHash",SqlDbType.NVarChar,50) { Direction = ParameterDirection.Input},
                new SqlParameter("securityStamp",SqlDbType.NVarChar,50) { Direction = ParameterDirection.Input},
                new SqlParameter("phoneNumber",SqlDbType.NVarChar,50) { Direction = ParameterDirection.Input},
                new SqlParameter("phoneNumberConfirmed",SqlDbType.Bit) { Direction = ParameterDirection.Input},
                new SqlParameter("twoFactorEnabled",SqlDbType.Bit) { Direction = ParameterDirection.Input},
                new SqlParameter("lockoutEndDateUtc",SqlDbType.DateTime) { Direction = ParameterDirection.Input},
                new SqlParameter("lockoutEnabled",SqlDbType.Bit) {Direction = ParameterDirection.Input },
                new SqlParameter("accessFailedCount",SqlDbType.Int) { Direction = ParameterDirection.Input},
                new SqlParameter("userName",SqlDbType.NVarChar,50) { Direction = ParameterDirection.Input}
            };
            return StoredProcedure(commandText, parameters);
        }


    }
}
