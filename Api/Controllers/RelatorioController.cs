using Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/relatorios")]
    public class RelatorioController : ControllerBase
    {
        private readonly RelatorioService _service;

        public RelatorioController(RelatorioService service)
        {
            _service = service;
        }

        [HttpGet("atrasados")]
        public async Task<IActionResult> Atrasados()
        {
            return Ok(await _service.ListarAtrasados());
        }

        [HttpGet("usuario/{id}")]
        public async Task<IActionResult> PorUsuario(int id)
        {
            return Ok(await _service.ListarPorUsuario(id));
        }

        [HttpGet("livro/{isbn}")]
        public async Task<IActionResult> PorLivro(string isbn)
        {
            return Ok(await _service.ListarPorLivro(isbn));
        }
    }
}
