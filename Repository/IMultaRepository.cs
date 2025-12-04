using Domain;

namespace Repository
{
    public interface IMultaRepository
    {
        Task<Multa?> GetByEmprestimoIdAsync(int emprestimoId);
        Task AddAsync(Multa multa);
        Task UpdateAsync(Multa multa);
    }
}
