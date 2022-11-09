using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiplosBancosDeDados.Core.Infra.CrossCutting
{
    public static class Inicializer
    {
        public static IServiceCollection AddSqlServerInstance(this IServiceCollection services, string dbAliasName)
        {
            services.AddHealthChecks().AddSqlServer(ConnectionStringData.GetConnectionString(dbAliasName), null, "SQL_" + dbAliasName, HealthStatus.Degraded);
            return services;
        }

        public static IServiceCollection AddConnectionStrings(this IServiceCollection services, IConfiguration configuration)
        {
            ConnectionStringData.BindConnectionStringData(configuration);
            return services;
        }
    }
}
