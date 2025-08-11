using ChatApp.Utitilies.Database;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data;


namespace Ecommerce.Infrastructure.Repositories
{
    public abstract class BaseRepository
    {
        readonly string ConnectionString;
        protected readonly DBHelperAsync _dbHelper;
        public BaseRepository(IConfiguration configuration)
        {
            try
            {
                ConnectionString = configuration.GetConnectionString("DefaultConnection");
                _dbHelper = new DBHelperAsync(ConnectionString);
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected async Task<List<TEntity>> GetListSP<TEntity>(string SPName)
        {
            try
            {
                return await _dbHelper.GetListSP<TEntity>(SPName);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected async Task<List<TEntity>> GetListSP<TEntity>(string SPName, DynamicParameters parameters)
        {
            try
            {
                return await _dbHelper.GetListSP<TEntity>(SPName, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected Task<int> ExecuteNonQuerySP(string SPName)
        {
            try
            {
                return _dbHelper.ExecuteNonQuerySP(SPName);
            }
            catch (Exception ex)
            {
                //NLogManager.PublishException(ex);
            }

            return null;
        }

        protected Task<int> ExecuteNonQuerySP(string SPName, DynamicParameters parameters)
        {
            try
            {
                return _dbHelper.ExecuteNonQuerySP(SPName, parameters);
            }
            catch (Exception ex)
            {
                //NLogManager.PublishException(ex);
            }

            return null;
        }

        protected Task<TEntity> GetInstanceSP<TEntity>(string SPName)
        {
            try
            {
                return _dbHelper.GetInstanceSP<TEntity>(SPName);
            }
            catch (Exception ex)
            {
                //NLogManager.PublishException(ex);
            }
            return null;
        }

        protected Task<TEntity> GetInstanceSP<TEntity>(string SPName, DynamicParameters parameters)
        {
            try
            {
                return _dbHelper.GetInstanceSP<TEntity>(SPName, parameters);
            }
            catch (Exception ex)
            {
                //NLogManager.PublishException(ex);
            }

            return null;
        }


        protected Task<TEntity> GetInstance<TEntity>(string strSQL, DynamicParameters parameters)
        {
            try
            {
                return _dbHelper.GetInstance<TEntity>(strSQL, parameters);
            }
            catch (Exception ex)
            {
                //NLogManager.PublishException(ex);
            }

            return null;
        }


        public virtual async Task<List<TEntity>> GetAllAsync<TEntity>()
        {
            var tableName = typeof(TEntity).Name;
            var procName = $"[dbo].[SP_{tableName}_GetAll]";
            return await GetListSP<TEntity>(procName);
        }

        public virtual async Task<TEntity?> GetByIDAsync<TEntity>(long id)
        {
            var tableName = typeof(TEntity).Name;
            var sqlCommand = $"SELECT * FROM [dbo].[{tableName}] where ID=@ID";

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@ID", id);

            return await GetInstance<TEntity>(sqlCommand, parameters);
        }

        public virtual async Task<int> InsertAsync<TEntity>(TEntity entity)
        {
            try
            {
                var tableName = typeof(TEntity).Name;
                var procName = $"SP_{tableName}_Create";

                DynamicParameters parameters = GetDynamicParametersCreate(entity);

                await ExecuteNonQuerySP(procName, parameters);

                return parameters.Get<int>("@ResponseStatus");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<int> UpdateAsync<TEntity>(TEntity entity)
        {
            try
            {
                var tableName = typeof(TEntity).Name;
                var procName = $"SP_{tableName}_Update";

                DynamicParameters parameters = GetDynamicParametersUpdate(entity);

                await ExecuteNonQuerySP(procName, parameters);

                return parameters.Get<int>("@ResponseStatus");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<int> DeleteAsync<TEntity>(long id)
        {
            var tableName = typeof(TEntity).Name;
            var procName = $"SP_{tableName}_Delete";

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@ID", id);
            parameters.Add("@ResponseStatus", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await ExecuteNonQuerySP(procName, parameters);
            return parameters.Get<int>("@ResponseStatus");
        }

        public virtual DynamicParameters GetDynamicParametersCreate<TEntity>(TEntity entity)
        {
            return new DynamicParameters();
        }
        public virtual DynamicParameters GetDynamicParametersUpdate<TEntity>(TEntity entity)
        {
            return new DynamicParameters();

        }

    }
}
