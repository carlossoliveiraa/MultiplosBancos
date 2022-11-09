using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiplosBancosDeDados.Core.Infra.CrossCutting
{
    public static class ParametersSqlServer
    {
        public static SqlParameter Create(string parameterName, object value)
        {
            return new SqlParameter(parameterName, value);
        }

        public static SqlParameter Create(string parameterName, object value, DbType dbType)
        {
            SqlParameter sqlParameter = Create(parameterName, value);
            sqlParameter.DbType = dbType;
            return sqlParameter;
        }

        public static SqlParameter Create(string parameterName, object value, DbType dbType, int size)
        {
            SqlParameter sqlParameter = Create(parameterName, value, dbType);
            sqlParameter.Size = size;
            return sqlParameter;
        }

        public static SqlParameter Create(string parameterName, object value, DbType dbType, SqlDbType sqlDbType, int size)
        {
            SqlParameter sqlParameter = Create(parameterName, value, dbType, size);
            sqlParameter.SqlDbType = sqlDbType;
            return sqlParameter;
        }

        public static SqlParameter Create(string parameterName, object value, DbType dbType, SqlDbType sqlDbType, int size, ParameterDirection direction)
        {
            SqlParameter sqlParameter = Create(parameterName, value, dbType, sqlDbType, size);
            sqlParameter.Direction = direction;
            return sqlParameter;
        }

        public static SqlParameter Create(string parameterName, object value, DbType dbType, SqlDbType sqlDbType, ParameterDirection direction)
        {
            SqlParameter sqlParameter = Create(parameterName, value, dbType);
            sqlParameter.SqlDbType = sqlDbType;
            sqlParameter.Direction = direction;
            return sqlParameter;
        }

        //public static SqlParameter CreateStructured(string parameterName, long[] ids)
        //{
        //    return CreateStructured(parameterName, ids.ToDataTable());
        //}

        //public static SqlParameter CreateStructured(string parameterName, int[] ids)
        //{
        //    return CreateStructured(parameterName, ids.ToDataTable());
        //}

        public static SqlParameter CreateStructured(string parameterName, DataTable dataTable)
        {
            SqlParameter sqlParameter = Create(parameterName, dataTable);
            sqlParameter.SqlDbType = SqlDbType.Structured;
            sqlParameter.Direction = ParameterDirection.Input;
            return sqlParameter;
        }
    }
}
