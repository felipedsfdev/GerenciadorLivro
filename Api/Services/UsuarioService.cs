using Domain;
using Repository;

namespace Api.Services
{
    public class UsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepo;
        private readonly IEmprestimoRepository _emprestimoRepo;

        public UsuarioService(
            IUsuarioRepository usuarioRepo,
            IEmprestimoRepository emprestimoRepo)
        {
            _usuarioRepo = usuarioRepo;
            _emprestimoRepo = emprestimoRepo;
        }

        public Task<IEnumerable<Usuario>> ListarTodos()
            => _usuarioRepo.GetAllAsync();

        public Task<Usuario?> ObterPorId(int id)
            => _usuarioRepo.GetByIdAsync(id);

        public async Task<Usuario> CriarUsuario(Usuario usuario)
        {
            usuario.DataCadastro = DateTime.UtcNow;
            await _usuarioRepo.AddAsync(usuario);
            return usuario;
        }

        public Task<int> ContarEmprestimosAtivos(int usuarioId)
            => _usuarioRepo.CountEmprestimosAtivosAsync(usuarioId);
    }
}
