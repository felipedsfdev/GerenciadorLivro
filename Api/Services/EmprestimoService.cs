using Domain;
using Repository;

namespace Api.Services
{
    public class EmprestimoService
    {
        private readonly ILivroRepository _livroRepo;
        private readonly IUsuarioRepository _usuarioRepo;
        private readonly IEmprestimoRepository _emprestimoRepo;
        private readonly IMultaRepository _multaRepo;

        public EmprestimoService(
            ILivroRepository livroRepo,
            IUsuarioRepository usuarioRepo,
            IEmprestimoRepository emprestimoRepo,
            IMultaRepository multaRepo)
        {
            _livroRepo = livroRepo;
            _usuarioRepo = usuarioRepo;
            _emprestimoRepo = emprestimoRepo;
            _multaRepo = multaRepo;
        }

        public async Task<Emprestimo> RegistrarEmprestimo(string isbn, int usuarioId)
        {
            var livro = await _livroRepo.GetByIsbnAsync(isbn)
                ?? throw new Exception("Livro não encontrado.");

            if (livro.Status != "DISPONIVEL")
                throw new Exception("Livro não está disponível.");

            var usuario = await _usuarioRepo.GetByIdAsync(usuarioId)
                ?? throw new Exception("Usuário não encontrado.");

            var ativos = await _usuarioRepo.CountEmprestimosAtivosAsync(usuarioId);
            if (ativos >= 3)
                throw new Exception("Usuário já possui 3 empréstimos ativos.");

            int prazo = usuario.Tipo.ToUpper() == "PROFESSOR" ? 14 : 7;

            var emprestimo = new Emprestimo
            {
                IsbnLivro = isbn,
                IdUsuario = usuarioId,
                DataEmprestimo = DateTime.UtcNow,
                DataPrevistaDevolucao = DateTime.UtcNow.AddDays(prazo),
                Status = "ATIVO"
            };

            await _emprestimoRepo.AddAsync(emprestimo);

            livro.Status = "EMPRESTADO";
            await _livroRepo.UpdateAsync(livro);

            return emprestimo;
        }

        public async Task RegistrarDevolucao(int emprestimoId)
        {
            var emp = await _emprestimoRepo.GetByIdAsync(emprestimoId)
                ?? throw new Exception("Empréstimo não encontrado.");

            if (emp.Status != "ATIVO")
                throw new Exception("Este empréstimo não está ativo.");

            emp.DataRealDevolucao = DateTime.UtcNow;

            // Cálculo da multa
            if (emp.DataRealDevolucao > emp.DataPrevistaDevolucao)
            {
                int atraso = (emp.DataRealDevolucao.Value - emp.DataPrevistaDevolucao).Days;
                decimal valor = atraso * 1m;

                await _multaRepo.AddAsync(new Multa
                {
                    IdEmprestimo = emprestimoId,
                    Valor = valor,
                    Status = "PENDENTE"
                });

                emp.Status = "ATRASADO";
            }
            else
            {
                emp.Status = "FINALIZADO";
            }

            await _emprestimoRepo.UpdateAsync(emp);

            var livro = await _livroRepo.GetByIsbnAsync(emp.IsbnLivro);
            if (livro != null)
            {
                livro.Status = "DISPONIVEL";
                await _livroRepo.UpdateAsync(livro);
            }
        }
    }
}
