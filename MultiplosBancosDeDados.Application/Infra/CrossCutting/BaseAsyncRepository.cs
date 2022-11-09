using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Reflection;
using System.Threading.Tasks;
using System;
using Dapper;
using System.Collections;

namespace MultiplosBancosDeDados.Core.Infra.CrossCutting
{
    public abstract class BaseAsyncRepository<TEntity, TConnection> where TEntity : class where TConnection : DbConnection, new()
    {
        private readonly string _ConnectionString;

        public BaseAsyncRepository(string connectionAlias)
        {
            _ConnectionString = ConnectionStringData.GetConnectionString(connectionAlias);
        }

        protected async Task<int> ExecuteWithoutReturnValueAsync(string commandText, DynamicParameters parameters, MethodBase methodBase, CommandType commandType = CommandType.StoredProcedure)
        {
            DbConnection connection = null;
            try
            {
                int result2;
                try
                {
                    await WriteBeginAsync(methodBase, parameters, commandText);
                    connection = await GetConnection();
                    int result = await connection.ExecuteAsync(commandText, parameters, null, null, commandType);
                    await $"Execucao do script {commandText} com {result} linhas afetadas.".WriteAsyncMethod(methodBase);
                    await WriteEndAsync(methodBase);
                    result2 = result;
                }
                catch (Exception ex)
                {
                    await WriteEndWithErrorAsync(methodBase, ex);
                    throw;
                }

                return result2;
            }
            finally
            {
                if (connection != null)
                {
                    await DisposeConnectionAsync(connection).ConfigureAwait(continueOnCapturedContext: true);
                }
            }
        }

        protected async Task<T> ExecuteWithReturnValueAsync<T>(string commandText, DynamicParameters parameters, MethodBase methodBase, CommandType commandType = CommandType.StoredProcedure)
        {
            DbConnection connection = null;
            try
            {
                T result2;
                try
                {
                    await WriteBeginAsync(methodBase, parameters, commandText);
                    connection = await GetConnection();
                    T result = await connection.ExecuteScalarAsync<T>(commandText, parameters, null, null, commandType);
                    await $"Execucao do script {commandText} com retorno {result}.".WriteAsyncMethod(methodBase);
                    await WriteEndAsync(methodBase);
                    result2 = result;
                }
                catch (Exception ex)
                {
                    await WriteEndWithErrorAsync(methodBase, ex);
                    throw;
                }

                return result2;
            }
            finally
            {
                if (connection != null)
                {
                    await DisposeConnectionAsync(connection).ConfigureAwait(continueOnCapturedContext: true);
                }
            }
        }

        protected async Task<List<TEntity>> GetAsync(string commandText, DynamicParameters parameters, MethodBase methodBase, CommandType commandType = CommandType.StoredProcedure)
        {
            DbConnection connection = null;
            try
            {
                List<TEntity> result;
                try
                {
                    List<TEntity> listaRegistros = new List<TEntity>();
                    await WriteBeginAsync(methodBase, parameters, commandText);
                    connection = await GetConnection();
                    List<TEntity> list = listaRegistros;
                    list.AddRange(await connection.QueryAsync<TEntity>(commandText, parameters, null, null, commandType));
                    await WriteEndAsync(methodBase, listaRegistros);
                    result = listaRegistros;
                }
                catch (Exception ex)
                {
                    await WriteEndWithErrorAsync(methodBase, ex);
                    throw;
                }

                return result;
            }
            finally
            {
                if (connection != null)
                {
                    await DisposeConnectionAsync(connection).ConfigureAwait(continueOnCapturedContext: true);
                }
            }
        }

        protected async Task<Tuple<List<TResult1>, List<TResult2>>> GetAsync<TResult1, TResult2>(string commandText, DynamicParameters parameters, MethodBase methodBase, CommandType commandType = CommandType.StoredProcedure) where TResult1 : TEntity where TResult2 : class
        {
            DbConnection connection = null;
            try
            {
                Tuple<List<TResult1>, List<TResult2>> result2;
                try
                {
                    List<TResult1> listaRegistros = new List<TResult1>();
                    List<TResult2> listaSecundaria = new List<TResult2>();
                    await WriteBeginAsync(methodBase, parameters, commandText);
                    connection = await GetConnection();
                    SqlMapper.GridReader result = await connection.QueryMultipleAsync(commandText, parameters, null, null, commandType);
                    if (result != null)
                    {
                        await "Iniciando Busca Primeira Query.".WriteAsyncMethod(methodBase);
                        List<TResult1> list = listaRegistros;
                        list.AddRange(await result.ReadAsync<TResult1>());
                        await "Iniciando Busca Segunda Query.".WriteAsyncMethod(methodBase);
                        List<TResult2> list2 = listaSecundaria;
                        list2.AddRange(await result.ReadAsync<TResult2>());
                    }

                    await WriteEndAsync(methodBase, listaRegistros, listaSecundaria);
                    result2 = new Tuple<List<TResult1>, List<TResult2>>(listaRegistros, listaSecundaria);
                }
                catch (Exception ex)
                {
                    await WriteEndWithErrorAsync(methodBase, ex);
                    throw;
                }

                return result2;
            }
            finally
            {
                if (connection != null)
                {
                    await DisposeConnectionAsync(connection).ConfigureAwait(continueOnCapturedContext: true);
                }
            }
        }

        protected async Task<Tuple<List<TResult1>, List<TResult2>, List<TResult3>>> GetAsync<TResult1, TResult2, TResult3>(string commandText, DynamicParameters parameters, MethodBase methodBase, CommandType commandType = CommandType.StoredProcedure) where TResult1 : TEntity where TResult2 : class where TResult3 : class
        {
            DbConnection connection = null;
            try
            {
                Tuple<List<TResult1>, List<TResult2>, List<TResult3>> result2;
                try
                {
                    await WriteBeginAsync(methodBase, parameters, commandText);
                    List<TResult1> listaRegistros = new List<TResult1>();
                    List<TResult2> listaSecundaria = new List<TResult2>();
                    List<TResult3> listaTerceira = new List<TResult3>();
                    SqlMapper.GridReader result = await connection.QueryMultipleAsync(commandText, parameters, null, null, commandType);
                    if (result != null)
                    {
                        await "Iniciando Busca Primeira Query.".WriteAsyncMethod(methodBase);
                        List<TResult1> list = listaRegistros;
                        list.AddRange(await result.ReadAsync<TResult1>());
                        await "Iniciando Busca Segunda Query.".WriteAsyncMethod(methodBase);
                        List<TResult2> list2 = listaSecundaria;
                        list2.AddRange(await result.ReadAsync<TResult2>());
                        await "Iniciando Busca Terceira Query.".WriteAsyncMethod(methodBase);
                        List<TResult3> list3 = listaTerceira;
                        list3.AddRange(await result.ReadAsync<TResult3>());
                    }

                    await WriteEndAsync(methodBase, listaRegistros, listaSecundaria, listaTerceira);
                    result2 = new Tuple<List<TResult1>, List<TResult2>, List<TResult3>>(listaRegistros, listaSecundaria, listaTerceira);
                }
                catch (Exception ex)
                {
                    await WriteEndWithErrorAsync(methodBase, ex);
                    throw;
                }

                return result2;
            }
            finally
            {
                if (connection != null)
                {
                    await DisposeConnectionAsync(connection).ConfigureAwait(continueOnCapturedContext: true);
                }
            }
        }

        protected async Task<List<TEntity>> GetAsync(string commandText, List<DbParameter> parameters, MethodBase methodBase, Func<IDataReader, TEntity> linha, CommandType commandType = CommandType.StoredProcedure)
        {
            DbCommand dbCommand = null;
            try
            {
                List<TEntity> result;
                try
                {
                    List<TEntity> listaRegistros = new List<TEntity>();
                    await WriteBeginAsync(methodBase, parameters, commandText);
                    dbCommand = await GetCommandAsync(commandText, parameters, commandType);
                    DbDataReader reader = await dbCommand.ExecuteReaderAsync(CommandBehavior.CloseConnection);
                    while (await reader.ReadAsync())
                    {
                        listaRegistros.Add(linha(reader));
                    }

                    await WriteEndAsync(methodBase, listaRegistros);
                    result = listaRegistros;
                }
                catch (Exception ex)
                {
                    await WriteEndWithErrorAsync(methodBase, ex);
                    throw;
                }

                return result;
            }
            finally
            {
                if (dbCommand != null)
                {
                    await DisposeConnectionAsync(dbCommand.Connection);
                }
            }
        }

        protected async Task<Tuple<List<TResult1>, List<TResult2>>> GetAsync<TResult1, TResult2>(string commandText, List<DbParameter> parameters, MethodBase methodBase, Func<IDataReader, TResult1> linha, Func<IDataReader, TResult2> linha2, CommandType commandType = CommandType.StoredProcedure) where TResult1 : TEntity where TResult2 : class
        {
            DbCommand dbCommand = null;
            try
            {
                Tuple<List<TResult1>, List<TResult2>> result;
                try
                {
                    List<TResult1> listaRegistros = new List<TResult1>();
                    List<TResult2> listaSecundaria = new List<TResult2>();
                    await WriteBeginAsync(methodBase, parameters, commandText);
                    dbCommand = await GetCommandAsync(commandText, parameters, commandType);
                    DbDataReader reader = await dbCommand.ExecuteReaderAsync(CommandBehavior.CloseConnection);
                    await "Iniciando Busca Primeira Query.".WriteAsyncMethod(methodBase);
                    while (await reader.ReadAsync())
                    {
                        listaRegistros.Add(linha(reader));
                    }

                    await "Iniciando Busca Segunda Query.".WriteAsyncMethod(methodBase);
                    if (await reader.NextResultAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            listaSecundaria.Add(linha2(reader));
                        }
                    }

                    await WriteEndAsync(methodBase, listaRegistros, listaSecundaria);
                    result = new Tuple<List<TResult1>, List<TResult2>>(listaRegistros, listaSecundaria);
                }
                catch (Exception ex)
                {
                    await WriteEndWithErrorAsync(methodBase, ex);
                    throw;
                }

                return result;
            }
            finally
            {
                if (dbCommand != null)
                {
                    await DisposeConnectionAsync(dbCommand.Connection);
                }
            }
        }

        protected async Task<Tuple<List<TResult1>, List<TResult2>, List<TResult3>>> GetAsync<TResult1, TResult2, TResult3>(string commandText, List<DbParameter> parameters, MethodBase methodBase, Func<IDataReader, TResult1> linha, Func<IDataReader, TResult2> linha2, Func<IDataReader, TResult3> linha3, CommandType commandType = CommandType.StoredProcedure) where TResult1 : TEntity where TResult2 : class where TResult3 : class
        {
            DbCommand dbCommand = null;
            try
            {
                Tuple<List<TResult1>, List<TResult2>, List<TResult3>> result;
                try
                {
                    List<TResult1> listaRegistros = new List<TResult1>();
                    List<TResult2> listaSecundaria = new List<TResult2>();
                    List<TResult3> listaTerceira = new List<TResult3>();
                    await WriteBeginAsync(methodBase, parameters, commandText);
                    dbCommand = await GetCommandAsync(commandText, parameters, commandType);
                    DbDataReader reader = await dbCommand.ExecuteReaderAsync(CommandBehavior.CloseConnection);
                    await "Iniciando Busca Primeira Query.".WriteAsyncMethod(methodBase);
                    while (await reader.ReadAsync())
                    {
                        listaRegistros.Add(linha(reader));
                    }

                    await "Iniciando Busca Segunda Query.".WriteAsyncMethod(methodBase);
                    if (await reader.NextResultAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            listaSecundaria.Add(linha2(reader));
                        }
                    }

                    await "Iniciando Busca Terceira Query.".WriteAsyncMethod(methodBase);
                    if (await reader.NextResultAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            listaTerceira.Add(linha3(reader));
                        }
                    }

                    await WriteEndAsync(methodBase, listaRegistros, listaSecundaria, listaTerceira);
                    result = new Tuple<List<TResult1>, List<TResult2>, List<TResult3>>(listaRegistros, listaSecundaria, listaTerceira);
                }
                catch (Exception ex)
                {
                    await WriteEndWithErrorAsync(methodBase, ex);
                    throw;
                }

                return result;
            }
            finally
            {
                if (dbCommand != null)
                {
                    await DisposeConnectionAsync(dbCommand.Connection);
                }
            }
        }

        protected async Task<TEntity> FindAsync(string commandText, DynamicParameters parameters, MethodBase methodBase, CommandType commandType = CommandType.StoredProcedure)
        {
            DbConnection connection = null;
            try
            {
                TEntity result;
                try
                {
                    await WriteBeginAsync(methodBase, parameters, commandText);
                    connection = await GetConnection();
                    TEntity entity = await connection.QueryFirstOrDefaultAsync<TEntity>(commandText, parameters, null, null, commandType);
                    await WriteEndAsync(methodBase, entity);
                    result = entity;
                }
                catch (Exception ex)
                {
                    await WriteEndWithErrorAsync(methodBase, ex);
                    throw;
                }

                return result;
            }
            finally
            {
                if (connection != null)
                {
                    await DisposeConnectionAsync(connection).ConfigureAwait(continueOnCapturedContext: true);
                }
            }
        }

        protected async Task<TEntity> FindAsync(string commandText, List<DbParameter> parameters, MethodBase methodBase, Func<IDataReader, TEntity> linha, CommandType commandType = CommandType.StoredProcedure)
        {
            DbCommand dbCommand = null;
            try
            {
                TEntity result;
                try
                {
                    await WriteBeginAsync(methodBase, parameters, commandText);
                    TEntity entity = null;
                    dbCommand = await GetCommandAsync(commandText, parameters, commandType);
                    DbDataReader reader = await dbCommand.ExecuteReaderAsync(CommandBehavior.CloseConnection);
                    if (await reader.ReadAsync())
                    {
                        entity = linha(reader);
                    }

                    await WriteEndAsync(methodBase, entity);
                    result = entity;
                }
                catch (Exception ex)
                {
                    await WriteEndWithErrorAsync(methodBase, ex);
                    throw;
                }

                return result;
            }
            finally
            {
                if (dbCommand != null)
                {
                    await DisposeConnectionAsync(dbCommand.Connection);
                }
            }
        }

        private async Task<DbCommand> GetCommandAsync(string commandText, List<DbParameter> listaParametros, CommandType commandType)
        {
            DbCommand command = (await GetConnection()).CreateCommand();
            command.CommandText = commandText;
            command.CommandType = commandType;
            if (listaParametros != null && listaParametros.Count > 0)
            {
                listaParametros.ForEach(delegate (DbParameter x)
                {
                    command.Parameters.Add(x);
                });
            }

            return command;
        }

        private async Task<DbConnection> GetConnection()
        {
            DbConnection dbConnection = new TConnection
            {
                ConnectionString = _ConnectionString
            };
            await dbConnection.OpenAsync();
            return dbConnection;
        }

        private async Task DisposeConnectionAsync(DbConnection dbConnection)
        {
            if (dbConnection != null)
            {
                if (dbConnection.State != 0)
                {
                    await dbConnection.CloseAsync().ConfigureAwait(continueOnCapturedContext: true);
                }

                await dbConnection.DisposeAsync().ConfigureAwait(continueOnCapturedContext: true);
            }
        }

        private async Task WriteBeginAsync(MethodBase methodBase, DynamicParameters parametros, string nomeStoreProcedure)
        {
            await "Início do processo".WriteAsyncMethod(methodBase);
            await (await GetParametersLogAsync(parametros, nomeStoreProcedure)).WriteAsyncMethod(methodBase);
        }

        private async Task WriteBeginAsync(MethodBase methodBase, List<DbParameter> parametros, string nomeStoreProcedure)
        {
            await "Início do processo".WriteAsyncMethod(methodBase);
            await (await GetParametersLogAsync(parametros, nomeStoreProcedure)).WriteAsyncMethod(methodBase);
        }

        private async Task WriteEndAsync(MethodBase methodBase)
        {
            await Task.Run(new Action(acaoLogar));
            void acaoLogar()
            {
                "Término do processo".WriteAsyncMethod(methodBase);
            }
        }

        private async Task WriteEndAsync(MethodBase methodBase, string mensagem)
        {
            await mensagem.WriteAsyncMethod(methodBase);
        }

        private async Task WriteEndAsync(MethodBase methodBase, IList results)
        {
            if (results == null || results.Count <= 0)
            {
                await WriteEndAsync(methodBase, "Término do processo NENHUM Registro Encontrado.");
            }
            else
            {
                await WriteEndAsync(methodBase, $"Término do processo com {results.Count} Registros.{Environment.NewLine}");
            }
        }

        private async Task WriteEndAsync(MethodBase methodBase, IList results, IList secondaryList)
        {
            if (results == null || results.Count <= 0)
            {
                await WriteEndAsync(methodBase, "Término do processo NENHUM Registro Encontrado na PRIMEIRA consulta.");
            }
            else
            {
                await WriteEndAsync(methodBase, $"Término da PRIMEIRA consulta com {results.Count} Registros.{Environment.NewLine}");
            }

            if (secondaryList == null || secondaryList.Count <= 0)
            {
                await WriteEndAsync(methodBase, "Término do processo NENHUM Registro Encontrado na SEGUNDA consulta.");
            }
            else
            {
                await WriteEndAsync(methodBase, $"Término da SEGUNDA consulta com {secondaryList.Count} Registros.{Environment.NewLine}");
            }
        }

        private async Task WriteEndAsync(MethodBase methodBase, IList results, IList secondaryList, IList thirdList)
        {
            if (results == null || results.Count <= 0)
            {
                await WriteEndAsync(methodBase, "Término do processo NENHUM Registro Encontrado na PRIMEIRA consulta.");
            }
            else
            {
                await WriteEndAsync(methodBase, $"Término da PRIMEIRA consulta com {results.Count} Registros.{Environment.NewLine}");
            }

            if (secondaryList == null || secondaryList.Count <= 0)
            {
                await WriteEndAsync(methodBase, "Término do processo NENHUM Registro Encontrado na SEGUNDA consulta.");
            }
            else
            {
                await WriteEndAsync(methodBase, $"Término da SEGUNDA consulta com {secondaryList.Count} Registros.{Environment.NewLine}");
            }

            if (thirdList == null || thirdList.Count <= 0)
            {
                await WriteEndAsync(methodBase, "Término do processo NENHUM Registro Encontrado na TERCEIRA consulta.");
            }
            else
            {
                await WriteEndAsync(methodBase, $"Término da TERCEIRA consulta com {thirdList.Count} Registros.{Environment.NewLine}");
            }
        }

        private async Task WriteEndAsync(MethodBase methodBase, TEntity entity)
        {
            if (entity != null)
            {
                await WriteEndAsync(methodBase, "Término do processo com o Registro ENCONTRADO");
            }
            else
            {
                await WriteEndAsync(methodBase, "Término do processo NENHUM Registro Encontrado.");
            }
        }

        private async Task WriteEndWithErrorAsync(MethodBase methodBase, Exception ex)
        {
            await ex.WriteWarningAsyncMethod("Término do processo com ERRO de Execucao", methodBase);
        }

        private async Task<string> GetParametersLogAsync(DynamicParameters parametros, string nomeStoreProcedure)
        {
            return await Task.Run(new Func<string>(acaoLogar));
            string acaoLogar()
            {
                string text = "Inicio da Execucao do SCRIPT " + nomeStoreProcedure + ". ";
                if (parametros != null && parametros.ParameterNames.AsList().Count > 0)
                {
                    text += "Execucao COM OS PARÂMETROS: [";
                    foreach (string parameterName in parametros.ParameterNames)
                    {
                        object obj = parametros.Get<object>(parameterName);
                        if (obj == null)
                        {
                            obj = "NULL";
                        }

                        text += $"{parameterName} : {obj}, ";
                    }

                    text = text[0..^2];
                    return text + "];";
                }

                return text + "Execucao SEM PARÂMETROS";
            }
        }

        private async Task<string> GetParametersLogAsync(List<DbParameter> parametros, string nomeStoreProcedure)
        {
            return await Task.Run(new Func<string>(acaoLogar));
            string acaoLogar()
            {
                string text = "Inicio da Execucao do SCRIPT " + nomeStoreProcedure + ". ";
                if (parametros != null && parametros.Count > 0)
                {
                    text += "Execucao COM OS PARÂMETROS: [";
                    foreach (DbParameter parametro in parametros)
                    {
                        object obj = parametro.Value;
                        if (obj == null)
                        {
                            obj = "NULL";
                        }

                        text += $"{parametro.ParameterName} : {obj}, ";
                    }

                    text = text[0..^2];
                    return text + "];";
                }

                return text + "Execucao SEM PARÂMETROS";
            }
        }
    }

    
}