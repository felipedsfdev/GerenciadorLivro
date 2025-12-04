using Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/emprestimos")]
    public class EmprestimoController : ControllerBase
    {
        private readonly EmprestimoService _service;

        public EmprestimoController(EmprestimoService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarEmprestimo([FromQuery] string isbn, [FromQuery] int usuarioId)
        {
            var emp = await _service.RegistrarEmprestimo(isbn, usuarioId);
            return Ok(emp);
        }

        [HttpPost("{id}/devolver")]
        public async Task<IActionResult> RegistrarDevolucao(int id)
        {
            await _service.RegistrarDevolucao(id);
            return NoContent();
        }
    }
}
