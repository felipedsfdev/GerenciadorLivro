using Domain;
using Repository;

namespace Api.Services
{
    public class MultaService
    {
        private readonly IMultaRepository _multaRepo;

        public MultaService(IMultaRepository multaRepo)
        {
            _multaRepo = multaRepo;
        }

        public Task<Multa?> ObterPorEmprestimo(int emprestimoId)
            => _multaRepo.GetByEmprestimoIdAsync(emprestimoId);

        public async Task PagarMulta(int emprestimoId)
        {
            var multa = await _multaRepo.GetByEmprestimoIdAsync(emprestimoId)
                ?? throw new Exception("Multa não encontrada.");

            multa.Status = "PAGA";
            await _multaRepo.UpdateAsync(multa);
        }
    }
}
