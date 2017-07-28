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
            throw new NotImplementedException();
        }
        public SqlParameter[] UserDeleteById(int userId)
        {
            throw new NotImplementedException();
        }
        public SqlParameter[] UserFindById(int userId)
        {
            throw new NotImplementedException();
        }
        public SqlParameter[] UserFindByName(string userName)
        {
            string commandText = "Identity.UserFindByName";
            SqlParameter[] parameters = new SqlParameter[]
            {
             new SqlParameter("userName", SqlDbType.NVarChar),
             new SqlParameter("id", SqlDbType.Int)
            };
            return StoredProcedure(commandText, parameters);
        }
        public SqlParameter[] UserUpdateById(AppUser user)
        {
            throw new NotImplementedException();
        }


    }
}
