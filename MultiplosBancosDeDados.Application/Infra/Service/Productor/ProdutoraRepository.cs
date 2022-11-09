using MultiplosBancosDeDados.Core.Domain;
using MultiplosBancosDeDados.Core.Infra.CrossCutting;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection;

namespace MultiplosBancosDeDados.Core.Infra.Service.Productor
{
    public class ProdutoraRepository : SqlServerAsyncRepository<Produtor>, IProdutoraRepository
    {
        public ProdutoraRepository() : base("Produtor")
        {
        }

        public IEnumerable<Produtor> ObterProdutoras()
        {
            return Get("PR_BuscarDados", new List<SqlParameter>()
            {

            }, MethodBase.GetCurrentMethod()).Result;
        }
    }
}
