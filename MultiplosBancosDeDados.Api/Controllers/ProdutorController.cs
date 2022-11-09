using Microsoft.AspNetCore.Mvc;
using MultiplosBancosDeDados.Core.Domain;
using MultiplosBancosDeDados.Core.Infra.Service.Consumer;
using MultiplosBancosDeDados.Core.Infra.Service.Productor;
using System.Collections.Generic;
using System.Linq;

namespace MultiplosBancosDeDados.Api.Controllers
{
    public class ProdutorController : Controller
    {

        private readonly IProdutoraRepository _produtoraRepository;
        private readonly IConsumidorRepository _consumidorRepository;
        public ProdutorController(IProdutoraRepository produtoraRepository, IConsumidorRepository consumidorRepository)
        {
            _produtoraRepository = produtoraRepository;
            _consumidorRepository = consumidorRepository;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("api/BuscaProdutor")]
        public IList<Produtor> GetProdutor()
        {
            return _produtoraRepository.ObterProdutoras().ToArray();
        }

        [HttpGet]
        [Route("api/BuscaConsumidor")]
        public IList<Consumidor> GetConsumidor()
        {
            return _consumidorRepository.ObterProdutoras().ToArray();
             
        }
    }
}
