using Api.Exceptions;
using Domain;
using Repository;

namespace Api.Services
{
    public class LivroService
    {
        private readonly ILivroRepository _livroRepository;

        public LivroService(ILivroRepository livroRepository)
        {
            _livroRepository = livroRepository;
        }

        // Criar livro com validação
        public async Task CadastrarLivroAsync(Livro livro)
        {
            // 1. Verificar se ISBN já existe
            var existente = await _livroRepository.GetByIsbnAsync(livro.ISBN);
            if (existente != null)
            {
                throw new RegraNegocioException("Já existe um livro cadastrado com esse ISBN.");
            }

            // 2. Definir data e status
            livro.DataCadastro = DateTime.Now;
            livro.Status = "DISPONIVEL";

            // 3. Salvar
            await _livroRepository.AddAsync(livro);
        }

        // Listar todos
        public async Task<IEnumerable<Livro>> ListarLivrosAsync()
        {
            return await _livroRepository.GetAllAsync();
        }

        // Buscar por ISBN
        public async Task<Livro?> BuscarPorIsbnAsync(string isbn)
        {
            return await _livroRepository.GetByIsbnAsync(isbn);
        }

        // Atualizar status
        public async Task AtualizarStatusAsync(string isbn, string novoStatus)
        {
            var livro = await _livroRepository.GetByIsbnAsync(isbn);
            if (livro == null)
                throw new RegraNegocioException("Livro não encontrado.");

            livro.Status = novoStatus;

            await _livroRepository.UpdateAsync(livro);
        }

        // Remover livro
        public async Task RemoverAsync(string isbn)
        {
            var livro = await _livroRepository.GetByIsbnAsync(isbn);
            if (livro == null)
                throw new RegraNegocioException("Livro não encontrado.");

            await _livroRepository.DeleteAsync(isbn);
        }
    }
}
