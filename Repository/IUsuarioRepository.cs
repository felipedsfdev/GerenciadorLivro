using Domain;

namespace Repository
{
    public interface IUsuarioRepository
    {
        Task<IEnumerable<Usuario>> GetAllAsync();
        Task<Usuario?> GetByIdAsync(int id);
        Task AddAsync(Usuario usuario);
        Task UpdateAsync(Usuario usuario);
        Task DeleteAsync(int id);

        // Regra de negócio
        Task<int> CountEmprestimosAtivosAsync(int usuarioId);
    }
}
