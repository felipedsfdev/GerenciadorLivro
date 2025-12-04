using Domain;

namespace Repository
{
    public interface ILivroRepository
    {
        Task<IEnumerable<Livro>> GetAllAsync();
        Task<Livro?> GetByIsbnAsync(string isbn);
        Task AddAsync(Livro livro);
        Task UpdateAsync(Livro livro);
        Task DeleteAsync(string isbn);
    }
}
