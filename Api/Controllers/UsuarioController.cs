using Api.Services;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/usuarios")]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioService _service;

        public UsuarioController(UsuarioService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.ListarTodos());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _service.ObterPorId(id);
            return user == null ? NotFound() : Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Usuario usuario)
        {
            var created = await _service.CriarUsuario(usuario);
            return CreatedAtAction(nameof(GetById), new { id = usuario.Id }, created);
        }

        [HttpGet("{id}/emprestimos-ativos")]
        public async Task<IActionResult> CountAtivos(int id)
        {
            int total = await _service.ContarEmprestimosAtivos(id);
            return Ok(total);
        }
    }
}
