using Dapper;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiplosBancosDeDados.Core.Infra.CrossCutting
{
    public static class SqlServerDatabaseFunction
    {
        public static DynamicParameters ConvertToDapperParameters(this List<SqlParameter> listaParametros)
        {
            DynamicParameters parameters = new DynamicParameters();
            listaParametros?.ForEach(delegate (SqlParameter x)
            {
                parameters.Add(x.ParameterName, x.Value, x.DbType, x.Direction, x.Size, null, null);
            });
            return parameters;
        }

        public static List<DbParameter> ConvertToDbParameter(this List<SqlParameter> listaParametros)
        {
            List<DbParameter> parameters = new List<DbParameter>();
            listaParametros?.ForEach(delegate (SqlParameter x)
            {
                parameters.Add(x);
            });
            return parameters;
        }
    }
}
