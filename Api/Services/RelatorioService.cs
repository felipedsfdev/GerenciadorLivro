using Domain;
using Repository;

namespace Api.Services
{
    public class RelatorioService
    {
        private readonly IEmprestimoRepository _emprestimoRepo;

        public RelatorioService(IEmprestimoRepository emprestimoRepo)
        {
            _emprestimoRepo = emprestimoRepo;
        }

        public Task<IEnumerable<Emprestimo>> ListarAtrasados()
            => _emprestimoRepo.GetAtrasadosAsync();

        public Task<IEnumerable<Emprestimo>> ListarPorUsuario(int usuarioId)
            => _emprestimoRepo.GetByUsuarioAsync(usuarioId);

        public Task<IEnumerable<Emprestimo>> ListarPorLivro(string isbn)
            => _emprestimoRepo.GetByLivroAsync(isbn);
    }
}
