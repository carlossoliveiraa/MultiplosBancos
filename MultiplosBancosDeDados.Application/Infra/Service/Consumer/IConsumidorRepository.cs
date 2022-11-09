using MultiplosBancosDeDados.Core.Domain;
using System.Collections.Generic;

namespace MultiplosBancosDeDados.Core.Infra.Service.Consumer
{
    public interface IConsumidorRepository
    {
        IEnumerable<Consumidor> ObterProdutoras();

    }
}
