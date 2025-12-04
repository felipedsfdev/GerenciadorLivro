using Api.Exceptions;
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

        // ------------------------------------------------------------
        // REGISTRAR EMPRÉSTIMO
        // POST /api/emprestimos?isbn=XXX&usuarioId=1
        // ------------------------------------------------------------
        [HttpPost]
        public async Task<IActionResult> RegistrarEmprestimo([FromQuery] string isbn, [FromQuery] int usuarioId)
        {
            try
            {
                await _service.RegistrarEmprestimo(isbn, usuarioId);
                return Ok(new { mensagem = "Empréstimo registrado com sucesso." });
            }
            catch (UsuarioInadimplenteException ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
            catch (RegraNegocioException ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { erro = ex.Message });
            }
        }

        // ------------------------------------------------------------
        // REGISTRAR DEVOLUÇÃO
        // POST /api/emprestimos/{id}/devolver
        // ------------------------------------------------------------
        [HttpPost("{id}/devolver")]
        public async Task<IActionResult> RegistrarDevolucao(int id)
        {
            try
            {
                await _service.RegistrarDevolucao(id);
                return Ok(new { mensagem = "Devolução registrada com sucesso." });
            }
            catch (EmprestimoNaoEncontradoException ex)
            {
                return NotFound(new { erro = ex.Message });
            }
            catch (RegraNegocioException ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { erro = ex.Message });
            }
        }
    }
}
