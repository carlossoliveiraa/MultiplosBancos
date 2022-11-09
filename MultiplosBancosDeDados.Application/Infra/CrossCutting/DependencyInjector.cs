using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MultiplosBancosDeDados.Core.Infra.Service.Consumer;
using MultiplosBancosDeDados.Core.Infra.Service.Productor;

namespace MultiplosBancosDeDados.Core.Infra.CrossCutting
{
    public static class DependencyInjector
    {
        public static IServiceCollection ConfigureInternalServices(this IServiceCollection services, IConfiguration configuration)
        {
            services               
                .AddRepositorys(configuration)
                .AddServices();

            return services;
        }
     
        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddTransient<IConsumidorRepository, ConsumidorRepository>();
            services.AddTransient<IProdutoraRepository, ProdutoraRepository>();

            return services;
        }
        private static IServiceCollection AddRepositorys(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddConnectionStrings(configuration);

            services.AddSqlServerInstance("Produtor");
            services.AddSqlServerInstance("Consumidor");

            //services.AddTransient<ITransmissaoRPSRepository, TransmissaoRPSRepository>();
            //services.AddTransient<ITransmissaoRPSFiscalRepository, TransmissaoRPSFiscalRepository>();
            //services.AddTransient<IPrestacaoContasRepository, PrestacaoContasRepository>();

            return services;
        }
    }
}
