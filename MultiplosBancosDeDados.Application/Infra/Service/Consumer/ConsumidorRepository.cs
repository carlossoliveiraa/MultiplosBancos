using MultiplosBancosDeDados.Core.Domain;
using MultiplosBancosDeDados.Core.Infra.CrossCutting;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection;

namespace MultiplosBancosDeDados.Core.Infra.Service.Consumer
{

    public class ConsumidorRepository : SqlServerAsyncRepository<Consumidor>, IConsumidorRepository
    {
        public ConsumidorRepository() : base("Consumidor")
        {
        }              

        public IEnumerable<Consumidor> ObterProdutoras()
        {
            return Get("PR_BuscarDados", new List<SqlParameter>()
            {

            }, MethodBase.GetCurrentMethod()).Result;
        }
    }
}
