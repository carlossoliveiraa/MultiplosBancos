using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;


namespace MultiplosBancosDeDados.Core.Infra.CrossCutting
{
    public static class ConnectionStringData
    {
        private static Dictionary<string, string> ConnectionStrings { get; set; }

        static ConnectionStringData()
        {
            ConnectionStrings = new Dictionary<string, string>();
        }

        public static string GetConnectionString(string aliasName)
        {
            if (ConnectionStrings == null || ConnectionStrings.Count == 0)
            {
                BindConnectionStringData();
            }

            if (ConnectionStrings.ContainsKey(aliasName))
            {
                return ConnectionStrings[aliasName];
            }

            throw new KeyNotFoundException("Database " + aliasName + " Not Found");
        }

        public static void BindConnectionStringData()
        {
            string text = ((!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"))) ? Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") : Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT"));
            BindConnectionStringData(new ConfigurationBuilder().SetBasePath(Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location)).AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).AddJsonFile("secrets/appsettings." + text + ".json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build());
        }

        public static void BindConnectionStringData(IConfiguration configuration)
        {
            ConnectionStrings = new Dictionary<string, string>();
            foreach (IConfigurationSection child in configuration.GetSection("ConnectionStrings").GetChildren())
            {
                ConnectionStrings.Add(child.Key, child.Value);
            }
        }
    }
}
