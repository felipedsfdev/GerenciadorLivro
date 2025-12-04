using Api.Services;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/livros")]
    public class LivroController : ControllerBase
    {
        private readonly LivroService _service;

        public LivroController(LivroService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.ListarTodos());
        }

        [HttpGet("{isbn}")]
        public async Task<IActionResult> GetByIsbn(string isbn)
        {
            var livro = await _service.ObterPorISBN(isbn);
            return livro == null ? NotFound() : Ok(livro);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Livro livro)
        {
            var created = await _service.CriarLivro(livro);
            return CreatedAtAction(nameof(GetByIsbn), new { isbn = livro.ISBN }, created);
        }

        [HttpPut("{isbn}/status")]
        public async Task<IActionResult> UpdateStatus(string isbn, [FromBody] string novoStatus)
        {
            await _service.AtualizarStatus(isbn, novoStatus);
            return NoContent();
        }
    }
}
