using Domain;
using Repository;

namespace Api.Services
{
    public class LivroService
    {
        private readonly ILivroRepository _livroRepo;

        public LivroService(ILivroRepository livroRepo)
        {
            _livroRepo = livroRepo;
        }

        public Task<IEnumerable<Livro>> ListarTodos()
            => _livroRepo.GetAllAsync();

        public async Task<Livro> CriarLivro(Livro livro)
        {
            livro.Status = "DISPONIVEL";
            livro.DataCadastro = DateTime.UtcNow;
            await _livroRepo.AddAsync(livro);
            return livro;
        }

        public Task<Livro?> ObterPorISBN(string isbn)
            => _livroRepo.GetByIsbnAsync(isbn);

        public async Task AtualizarStatus(string isbn, string novoStatus)
        {
            var livro = await _livroRepo.GetByIsbnAsync(isbn)
                ?? throw new Exception("Livro não encontrado.");

            if (livro.Status == "EMPRESTADO" && novoStatus == "RESERVADO")
                throw new Exception("Livro emprestado não pode ser reservado.");

            livro.Status = novoStatus;
            await _livroRepo.UpdateAsync(livro);
        }
    }
}
