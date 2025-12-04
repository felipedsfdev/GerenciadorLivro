using Domain;

namespace Repository
{
    public interface IEmprestimoRepository
    {
        Task<Emprestimo?> GetByIdAsync(int id);
        Task<IEnumerable<Emprestimo>> GetAllAsync();
        Task AddAsync(Emprestimo emprestimo);
        Task UpdateAsync(Emprestimo emprestimo);

        // Relatórios
        Task<IEnumerable<Emprestimo>> GetAtrasadosAsync();
        Task<IEnumerable<Emprestimo>> GetByUsuarioAsync(int usuarioId);
        Task<IEnumerable<Emprestimo>> GetByLivroAsync(string isbn);
    }
}
