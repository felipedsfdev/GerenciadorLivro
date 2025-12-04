using Api.Exceptions;
using Domain;
using Repository;

namespace Api.Services
{
    public class EmprestimoService
    {
        private readonly ILivroRepository _livroRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IEmprestimoRepository _emprestimoRepository;
        private readonly IMultaRepository _multaRepository;

        public EmprestimoService(
            ILivroRepository livroRepository,
            IUsuarioRepository usuarioRepository,
            IEmprestimoRepository emprestimoRepository,
            IMultaRepository multaRepository)
        {
            _livroRepository = livroRepository;
            _usuarioRepository = usuarioRepository;
            _emprestimoRepository = emprestimoRepository;
            _multaRepository = multaRepository;
        }


        // REGISTRAR EMPRÉSTIMO
  
        public async Task RegistrarEmprestimo(string isbn, int usuarioId)
        {
            // 1) Validar livro
            var livro = await _livroRepository.GetByIsbnAsync(isbn);
            if (livro == null)
                throw new RegraNegocioException("Livro não encontrado.");

            if (livro.Status != "DISPONIVEL")
                throw new RegraNegocioException("Livro não está disponível para empréstimo.");

            // 2) Validar usuário
            var usuario = await _usuarioRepository.GetByIdAsync(usuarioId);
            if (usuario == null)
                throw new RegraNegocioException("Usuário não encontrado.");

            // 3) Bloqueio por multa pendente
            var emprestimosUsuario = await _emprestimoRepository.GetByUsuarioAsync(usuarioId);
            foreach (var emp in emprestimosUsuario)
            {
                var multa = await _multaRepository.GetByEmprestimoIdAsync(emp.Id);
                if (multa != null && multa.Status == "PENDENTE")
                {
                    throw new UsuarioInadimplenteException(
                        "Usuário possui multa pendente e não pode realizar novos empréstimos.");
                }
            }

            // 4) Limite de 3 empréstimos ativos
            var ativos = await _usuarioRepository.CountEmprestimosAtivosAsync(usuarioId);
            if (ativos >= 3)
                throw new RegraNegocioException("Usuário já possui 3 empréstimos ativos.");

            // 5) Definir prazo por tipo de usuário
            int prazoDias = usuario.Tipo.ToUpper() == "PROFESSOR" ? 14 : 7;

            var emprestimo = new Emprestimo
            {
                IsbnLivro = isbn,
                IdUsuario = usuarioId,
                DataEmprestimo = DateTime.UtcNow,
                DataPrevistaDevolucao = DateTime.UtcNow.AddDays(prazoDias),
                Status = "ATIVO"
            };

            await _emprestimoRepository.AddAsync(emprestimo);

            // 6) Atualizar status do livro para EMPRESTADO
            livro.Status = "EMPRESTADO";
            await _livroRepository.UpdateAsync(livro);
        }

        // --------------------------------------------------------------------
        // REGISTRAR DEVOLUÇÃO
        // --------------------------------------------------------------------
        public async Task RegistrarDevolucao(int emprestimoId)
        {
            var emprestimo = await _emprestimoRepository.GetByIdAsync(emprestimoId);

            if (emprestimo == null)
                throw new EmprestimoNaoEncontradoException("Empréstimo não encontrado.");

            if (emprestimo.Status != "ATIVO")
                throw new RegraNegocioException("Não é possível devolver um empréstimo que não está ativo.");

            emprestimo.DataRealDevolucao = DateTime.UtcNow;

            // 1) Verifica atraso
            if (emprestimo.DataRealDevolucao > emprestimo.DataPrevistaDevolucao)
            {
                int diasAtraso = (emprestimo.DataRealDevolucao.Value.Date - emprestimo.DataPrevistaDevolucao.Date).Days;

                if (diasAtraso > 0)
                {
                    var multa = new Multa
                    {
                        IdEmprestimo = emprestimo.Id,
                        Valor = diasAtraso * 1.0m,  // R$ 1,00 por dia
                        Status = "PENDENTE"
                    };

                    await _multaRepository.AddAsync(multa);
                }

                emprestimo.Status = "ATRASADO";
            }
            else
            {
                emprestimo.Status = "FINALIZADO";
            }

            await _emprestimoRepository.UpdateAsync(emprestimo);

            // 2) Atualizar livro para DISPONIVEL
            var livro = await _livroRepository.GetByIsbnAsync(emprestimo.IsbnLivro);
            if (livro != null)
            {
                livro.Status = "DISPONIVEL";
                await _livroRepository.UpdateAsync(livro);
            }
        }
    }
}
