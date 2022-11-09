using MultiplosBancosDeDados.Core.Domain;
using System.Collections.Generic;

namespace MultiplosBancosDeDados.Core.Infra.Service.Productor
{
    public interface IProdutoraRepository
    {
        IEnumerable<Produtor> ObterProdutoras();
    }
}
