using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Newtonsoft.Json;

namespace ChatApp.Utilities.Database
{
    /// <summary>
    /// Helper for executing async database operations using Dapper.
    /// Provides built-in logging and consistent exception handling.
    /// </summary>
    public class DBHelperAsync
    {
        private string _connectionString;

        public DBHelperAsync(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public string ConnectionString
        {
            get => _connectionString;
            set => _connectionString = value ?? throw new ArgumentNullException(nameof(value));
        }

        #region Connection Management

        private string FixConnectionString(string connectionString, bool pooling)
        {
            var parts = connectionString.Split(';', StringSplitOptions.RemoveEmptyEntries);
            var sb = new StringBuilder();

            foreach (var part in parts)
            {
                var lower = part.Trim().ToLowerInvariant();
                if (lower.StartsWith("pooling=") ||
                    lower.StartsWith("min pool size=") ||
                    lower.StartsWith("max pool size=") ||
                    lower.StartsWith("connect timeout="))
                    continue;

                sb.Append(part.Trim()).Append(';');
            }

            sb.Append(pooling
                ? "Pooling=true;Min Pool Size=5;Max Pool Size=25;Connect Timeout=5;"
                : "Pooling=false;Connect Timeout=10;");

            return sb.ToString();
        }

        private async Task<SqlConnection> CreateConnectionAsync()
        {
            if (string.IsNullOrWhiteSpace(_connectionString))
                throw new InvalidOperationException("Connection string cannot be null or empty.");

            try
            {
                var conn = new SqlConnection(FixConnectionString(_connectionString, true));
                await conn.OpenAsync();
                return conn;
            }
            catch (Exception ex)
            {
                var fallback = new SqlConnection(FixConnectionString(_connectionString, false));
                await fallback.OpenAsync();
                return fallback;
            }
        }

        #endregion

        #region Generic Query Executor with Error Handling

        private async Task<TResult> ExecuteAsync<TResult>(
            Func<SqlConnection, Task<TResult>> action,
            string operationDescription)
        {
            await using var conn = await CreateConnectionAsync();
            try
            {
                return await action(conn);
            }
            catch (SqlException ex)
            {
                throw new DatabaseExecutionException($"SQL error during {operationDescription}: {ex.Message}", ex);
            }
            catch (TimeoutException ex)
            {
                throw new DatabaseExecutionException($"Database timeout during {operationDescription}", ex);
            }
            catch (Exception ex)
            {
                throw new DatabaseExecutionException($"Unexpected error during {operationDescription}", ex);
            }
        }

        private async Task<int> ExecuteAsync(Func<SqlConnection, Task<int>> action, string operationDescription)
        {
            await using var conn = await CreateConnectionAsync();
            try
            {
                return await action(conn);
            }
            catch (SqlException ex)
            {
                throw new DatabaseExecutionException($"SQL error during {operationDescription}: {ex.Message}", ex);
            }
            catch (TimeoutException ex)
            {
                throw new DatabaseExecutionException($"Database timeout during {operationDescription}", ex);
            }
            catch (Exception ex)
            {
                throw new DatabaseExecutionException($"Unexpected error during {operationDescription}", ex);
            }
        }

        #endregion

        #region Public Query Methods

        public Task<List<T>> GetListAsync<T>(
            string sql,
            DynamicParameters? param = null,
            CommandType cmdType = CommandType.Text)
            => ExecuteAsync(async conn =>
            {
                var result = await conn.QueryAsync<T>(sql, param, commandType: cmdType);
                return result.ToList();
            }, $"GetListAsync<{typeof(T).Name}> [{cmdType}]");

        public Task<T?> GetInstanceAsync<T>(
            string sql,
            DynamicParameters? param = null,
            CommandType cmdType = CommandType.Text)
            => ExecuteAsync(conn => conn.QueryFirstOrDefaultAsync<T>(sql, param, commandType: cmdType),
                $"GetInstanceAsync<{typeof(T).Name}> [{cmdType}]");

        public Task<int> ExecuteNonQueryAsync(
            string sql,
            DynamicParameters? param = null,
            CommandType cmdType = CommandType.Text)
            => ExecuteAsync(conn => conn.ExecuteAsync(sql, param, commandType: cmdType),
                $"ExecuteNonQueryAsync [{cmdType}]");

        #endregion

        #region JSON Query Support

        private class JsonData
        {
            public int? TotalRows { get; set; }
            public string? JColumn { get; set; }
        }

        /// <summary>
        /// Data returned must include a column named "JColumn" as JSON content.
        /// </summary>
        public async Task<(List<object> Data, int? TotalRows)> GetListJsonAsync(
            string sql, CommandType cmdType, DynamicParameters? param = null)
        {
            return await ExecuteAsync(async conn =>
            {
                var data = await conn.QueryAsync<JsonData>(sql, param, commandType: cmdType);
                var result = new List<object>();
                int? totalRows = 0;

                foreach (var item in data)
                {
                    if (!string.IsNullOrEmpty(item.JColumn))
                        result.Add(JsonConvert.DeserializeObject(item.JColumn)!);
                    totalRows = item.TotalRows ?? totalRows;
                }

                return (result, totalRows);
            }, "GetListJsonAsync");
        }

        #endregion
    }

    /// <summary>
    /// Custom exception type for database execution failures.
    /// </summary>
    public class DatabaseExecutionException : Exception
    {
        public DatabaseExecutionException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
