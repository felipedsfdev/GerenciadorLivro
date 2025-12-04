using Api.Exceptions;
using Api.Services;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/livros")]
    public class LivroController : ControllerBase
    {
        private readonly LivroService _livroService;

        public LivroController(LivroService livroService)
        {
            _livroService = livroService;
        }

        [HttpPost]
        public async Task<IActionResult> Criar(Livro livro)
        {
            try
            {
                await _livroService.CadastrarLivroAsync(livro);
                return Ok("Livro cadastrado com sucesso.");
            }
            catch (RegraNegocioException ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            return Ok(await _livroService.ListarLivrosAsync());
        }

        [HttpGet("{isbn}")]
        public async Task<IActionResult> Buscar(string isbn)
        {
            var livro = await _livroService.BuscarPorIsbnAsync(isbn);
            if (livro == null)
                return NotFound();

            return Ok(livro);
        }

        [HttpDelete("{isbn}")]
        public async Task<IActionResult> Remover(string isbn)
        {
            try
            {
                await _livroService.RemoverAsync(isbn);
                return Ok("Livro removido.");
            }
            catch (RegraNegocioException ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
        }
    }
}
