using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Newtonsoft.Json;

namespace ChatApp.Utitilies.Database
{
    public class DBHelperAsync
    {
        private string _connectionString = "";

        public string FixConnectionString(string connectionString, bool pooling)
        {
            var connectionParts = connectionString.Split(';');
            var fixedConnectionString = new StringBuilder();

            foreach (var part in connectionParts)
            {
                if (string.IsNullOrWhiteSpace(part)) continue;

                if (part.ToLower().StartsWith("pooling=") ||
                    part.ToLower().StartsWith("min pool size=") ||
                    part.ToLower().StartsWith("max pool size=") ||
                    part.ToLower().StartsWith("connect timeout="))
                {
                    continue;
                }

                fixedConnectionString.Append(part).Append(';');
            }

            if (pooling)
            {
                fixedConnectionString.Append("Pooling=true;Min Pool Size=5;Max Pool Size=25;Connect Timeout=5;");
            }
            else
            {
                fixedConnectionString.Append("Pooling=false;Connect Timeout=10;");
            }

            return fixedConnectionString.ToString();
        }

        
        public DBHelperAsync() { }

        public DBHelperAsync(string connectionString)
        {
            _connectionString = connectionString;
        }

        public string ConnectionString
        {
            get => _connectionString;
            set => _connectionString = value;
        }

        private SqlConnection _connectionToDB;

        public SqlConnection ConnectionToDB
        {
            get => _connectionToDB;
            private set => _connectionToDB = value;
        }


        public async Task Open()
        {
            if (string.IsNullOrWhiteSpace(_connectionString))
            {
                throw new InvalidOperationException("Connection string cannot be null or empty.");
            }

            _connectionToDB = await OpenConnection();
        }

        public async Task<SqlConnection> OpenConnection(string connectionString)
        {
            _connectionString = connectionString;
            return await OpenConnection();
        }

        public async Task<SqlConnection> OpenConnection()
        {
            if (string.IsNullOrWhiteSpace(_connectionString))
            {
                throw new InvalidOperationException("Connection string cannot be null or empty.");
            }

            try
            {
                var connection = new SqlConnection(FixConnectionString(_connectionString, true));
                await connection.OpenAsync();
                return connection;
            }
            catch (Exception)
            {
                // Retry with pooling disabled in case of max pool issue
                var connection = new SqlConnection(FixConnectionString(_connectionString, false));
                await connection.OpenAsync();
                return connection;
            }
        }

        public Task CloseConnection(SqlConnection connection)
        {

            if (connection != null && connection.State == ConnectionState.Open)
            {
                return  connection.CloseAsync();
            }

            return Task.CompletedTask;
        }

        public void Close()
        {
            _connectionToDB?.Close();
        }

        #region [GetList]
        public async Task<List<T>> GetList<T>(string strSQL)
        {
            try
            {
                await Open();
                var result = await _connectionToDB.QueryAsync<T>(strSQL);
                return result.ToList();
            }
            catch (SqlException ex)
            {
                //await CloseConnection(_connectionToDB);
                throw new Exception("An error occurred while executing the stored procedure.", ex);
            }
            finally
            {
                await CloseConnection(_connectionToDB);
            }
        }

        public async Task<List<T>> GetList<T>(string strSQL, DynamicParameters parameters)
        {
            try
            {
                await Open();
                var result = await _connectionToDB.QueryAsync<T>(strSQL, parameters);
                return result.ToList();
            }
            catch (SqlException ex)
            {
                //await CloseConnection(_connectionToDB);
                throw new Exception("An error occurred while executing the stored procedure.", ex);
            }
            finally
            {
                await CloseConnection(_connectionToDB);
            }
        } 
        #endregion

        #region [GetListSP]

        public async Task<List<T>> GetListSP<T>(string SPName)
        {
            try
            {
                await Open();
                var result = await _connectionToDB.QueryAsync<T>(SPName, commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
            catch (SqlException ex)
            {
                //await CloseConnection(_connectionToDB);
                throw new Exception("An error occurred while executing the stored procedure.", ex);
            }
            finally
            {
                await CloseConnection(_connectionToDB);
            }
        }

        public async Task<List<T>> GetListSP<T>(string SPName, DynamicParameters parameters)
        {
            try
            {
                await Open();
                var result = await _connectionToDB.QueryAsync<T>(SPName, parameters, commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
            catch (SqlException ex)
            {
                //await CloseConnection(_connectionToDB);
                throw new Exception("An error occurred while executing the stored procedure.", ex);
            }
            finally
            {
                await CloseConnection(_connectionToDB);
            }
        }
        #endregion

        #region [GetInstance]
        public async Task<T> GetInstance<T>(string strSQL)
        {
            try
            {
                await Open();
                var result = await _connectionToDB.QueryFirstOrDefaultAsync<T>(strSQL);
                return result;
            }
            catch (SqlException ex)
            {
                //await CloseConnection(_connectionToDB);
                throw new Exception("An error occurred while executing the stored procedure.", ex);
            }
            finally
            {
                await CloseConnection(_connectionToDB);
            }
        }

        public async Task<T> GetInstance<T>(string strSQL, DynamicParameters parameters)
        {
            try
            {
                await Open();
                var result = await _connectionToDB.QueryFirstOrDefaultAsync<T>(strSQL, parameters);
                return result;
            }
            catch (SqlException ex)
            {
                //await CloseConnection(_connectionToDB);
                throw new Exception("An error occurred while executing the stored procedure.", ex);
            }
            finally
            {
                await CloseConnection(_connectionToDB);
            }
        }

        #endregion

        #region[GetInstanceSP]

        public async Task<T> GetInstanceSP<T>(string SPName)
        {
            try
            {
                await Open();
                var result = await _connectionToDB.QueryFirstOrDefaultAsync<T>(SPName, commandType: CommandType.StoredProcedure);
                return result;
            }
            catch (SqlException ex)
            {
                //await CloseConnection(_connectionToDB);
                throw new Exception("An error occurred while executing the stored procedure.", ex);
            }
            finally
            {
                await CloseConnection(_connectionToDB);
            }
        }

        public async Task<T> GetInstanceSP<T>(string SPName, DynamicParameters parameters)
        {
            try
            {
                await Open();
                var result = await _connectionToDB.QueryFirstOrDefaultAsync<T>(SPName, parameters, commandType: CommandType.StoredProcedure);
                return result;
            }
            catch (SqlException ex)
            {
                //await CloseConnection(_connectionToDB);
                throw new Exception("An error occurred while executing the stored procedure.", ex);
            }
            finally
            {
                await CloseConnection(_connectionToDB);
            }
        }

        #endregion

        #region [ExecuteNonQuerySP]
        public async Task<int> ExecuteNonQuerySP(string SPName)
        {
            try
            {
                await Open();
                var result = await _connectionToDB.ExecuteAsync(SPName, commandType: CommandType.StoredProcedure);
                return result;
            }
            catch (SqlException ex)
            {
                //await CloseConnection(_connectionToDB);
                throw new Exception("An error occurred while executing the stored procedure.", ex);
            }
            finally
            {
                await CloseConnection(_connectionToDB);
            }
        }

        public async Task<int> ExecuteNonQuerySP(string SPName, DynamicParameters parameters)
        {
            try
            {
                await Open();
                var result = await _connectionToDB.ExecuteAsync(SPName, parameters, commandType: CommandType.StoredProcedure);
                return result;
            }
            catch (SqlException ex)
            {
                //await CloseConnection(_connectionToDB);
                throw new Exception("An error occurred while executing the stored procedure.", ex);
            }
            finally
            {
                await CloseConnection(_connectionToDB);
            }
        }

        #endregion

        #region[GetListJsonSP]
        public class JsonData
        {
            public int? TotalRows { get; set; }
            public string? JColumn { get; set; }
        }

        /// <summary>
        /// Data from the database must have a column named "JColumn" with return type json
        /// </summary>
        /// <param name="strSQL"></param>
        /// <param name="cType"></param>
        /// <param name="pars"></param>
        /// <returns></returns>
        public async Task<(List<Object> data, int? totalRows)> GetListJsonSP(string strSQL, CommandType cType, DynamicParameters? parameters = null)
        {
            int? totalRows = 0;
            List<Object> retData = new List<Object>();
            List<JsonData> data = null;
            if (cType == CommandType.StoredProcedure)
            {
                if (parameters != null) {
                    data = await GetListSP<JsonData>(strSQL, parameters);
                }
                else
                {
                    data = await GetListSP<JsonData>(strSQL);
                }
            }
            else
            {
                data = await GetList<JsonData>(strSQL);
            }
            if (data != null)
            {
                foreach (var item in data)
                {
                    retData.Add(JsonConvert.DeserializeObject(item.JColumn));
                    totalRows = item.TotalRows;
                }
            }
            return (retData, totalRows);
        }
        #endregion
    }
}
