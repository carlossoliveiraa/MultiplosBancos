using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MultiplosBancosDeDados.Core.Infra.CrossCutting
{
    public abstract class SqlServerAsyncRepository<TEntity> : BaseAsyncRepository<TEntity, SqlConnection> where TEntity : class
    {
        public SqlServerAsyncRepository(string connectionAlias)
            : base(connectionAlias)
        {
        }

        protected async Task<int> ExecuteAsync(string commandText, List<SqlParameter> parameters, MethodBase methodBase, CommandType commandType = CommandType.StoredProcedure)
        {
            return await ExecuteWithoutReturnValueAsync(commandText, parameters.ConvertToDapperParameters(), methodBase, commandType);
        }

        protected async Task<T> ExecuteScalar<T>(string commandText, List<SqlParameter> parameters, MethodBase methodBase, CommandType commandType = CommandType.StoredProcedure)
        {
            return await ExecuteWithReturnValueAsync<T>(commandText, parameters.ConvertToDapperParameters(), methodBase, commandType);
        }

        

        protected async Task<List<TEntity>> Get(string commandText, List<SqlParameter> parameters, MethodBase methodBase, CommandType commandType = CommandType.StoredProcedure)
        {
            return await GetAsync(commandText, parameters.ConvertToDapperParameters(), methodBase, commandType);
        }

        protected async Task<List<TEntity>> Get(string commandText, List<SqlParameter> parameters, MethodBase methodBase, Func<IDataReader, TEntity> linha, CommandType commandType = CommandType.StoredProcedure)
        {
            return await GetAsync(commandText, parameters.ConvertToDbParameter(), methodBase, linha, commandType);
        }

        protected async Task<Tuple<List<TResult1>, List<TResult2>>> Get<TResult1, TResult2>(string commandText, List<SqlParameter> parameters, MethodBase methodBase, CommandType commandType = CommandType.StoredProcedure) where TResult1 : TEntity where TResult2 : class
        {
            return await GetAsync<TResult1, TResult2>(commandText, parameters.ConvertToDapperParameters(), methodBase, commandType);
        }

        protected async Task<Tuple<List<TResult1>, List<TResult2>>> Get<TResult1, TResult2>(string commandText, List<SqlParameter> parameters, MethodBase methodBase, Func<IDataReader, TResult1> linha, Func<IDataReader, TResult2> linha2, CommandType commandType = CommandType.StoredProcedure) where TResult1 : TEntity where TResult2 : class
        {
            return await GetAsync(commandText, parameters.ConvertToDbParameter(), methodBase, linha, linha2, commandType);
        }

        protected async Task<Tuple<List<TResult1>, List<TResult2>, List<TResult3>>> Get<TResult1, TResult2, TResult3>(string commandText, List<SqlParameter> parameters, MethodBase methodBase, CommandType commandType = CommandType.StoredProcedure) where TResult1 : TEntity where TResult2 : class where TResult3 : class
        {
            return await GetAsync<TResult1, TResult2, TResult3>(commandText, parameters.ConvertToDapperParameters(), methodBase, commandType);
        }

        protected async Task<Tuple<List<TResult1>, List<TResult2>, List<TResult3>>> Get<TResult1, TResult2, TResult3>(string commandText, List<SqlParameter> parameters, MethodBase methodBase, Func<IDataReader, TResult1> linha, Func<IDataReader, TResult2> linha2, Func<IDataReader, TResult3> linha3, CommandType commandType = CommandType.StoredProcedure) where TResult1 : TEntity where TResult2 : class where TResult3 : class
        {
            return await GetAsync(commandText, parameters.ConvertToDbParameter(), methodBase, linha, linha2, linha3, commandType);
        }

        protected async Task<TEntity> Find(string commandText, List<SqlParameter> parameters, MethodBase methodBase, CommandType commandType = CommandType.StoredProcedure)
        {
            return await FindAsync(commandText, parameters.ConvertToDapperParameters(), methodBase, commandType);
        }

        protected async Task<TEntity> Find(string commandText, List<SqlParameter> parameters, MethodBase methodBase, Func<IDataReader, TEntity> linha, CommandType commandType = CommandType.StoredProcedure)
        {
            return await FindAsync(commandText, parameters.ConvertToDbParameter(), methodBase, linha, commandType);
        }
    }
}
