using Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/multas")]
    public class MultaController : ControllerBase
    {
        private readonly MultaService _service;

        public MultaController(MultaService service)
        {
            _service = service;
        }

        [HttpGet("{emprestimoId}")]
        public async Task<IActionResult> GetByEmprestimoId(int emprestimoId)
        {
            var multa = await _service.ObterPorEmprestimo(emprestimoId);
            return multa == null ? NotFound() : Ok(multa);
        }

        [HttpPost("{emprestimoId}/pagar")]
        public async Task<IActionResult> Pagar(int emprestimoId)
        {
            await _service.PagarMulta(emprestimoId);
            return NoContent();
        }
    }
}
