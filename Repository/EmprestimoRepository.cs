using Dapper;
using Domain;
using Microsoft.Data.Sqlite;

namespace Repository
{
    public class EmprestimoRepository : IEmprestimoRepository
    {
        private readonly string _connectionString;

        public EmprestimoRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<Emprestimo?> GetByIdAsync(int id)
        {
            using var connection = new SqliteConnection(_connectionString);

            return await connection.QuerySingleOrDefaultAsync<Emprestimo>(
                "SELECT * FROM Emprestimo WHERE Id = @id",
                new { id }
            );
        }

        public async Task<IEnumerable<Emprestimo>> GetAllAsync()
        {
            using var connection = new SqliteConnection(_connectionString);

            return await connection.QueryAsync<Emprestimo>("SELECT * FROM Emprestimo");
        }

        public async Task AddAsync(Emprestimo emprestimo)
        {
            using var connection = new SqliteConnection(_connectionString);

            const string sql = @"
                INSERT INTO Emprestimo (ISBNLivro, UsuarioId, DataEmprestimo, DataPrevistaDevolucao, DataRealDevolucao, Status)
                VALUES (@ISBNLivro, @UsuarioId, @DataEmprestimo, @DataPrevistaDevolucao, @DataRealDevolucao, @Status);
            ";

            await connection.ExecuteAsync(sql, emprestimo);
        }

        public async Task UpdateAsync(Emprestimo emprestimo)
        {
            using var connection = new SqliteConnection(_connectionString);

            const string sql = @"
                UPDATE Emprestimo SET
                    ISBNLivro = @ISBNLivro,
                    UsuarioId = @UsuarioId,
                    DataEmprestimo = @DataEmprestimo,
                    DataPrevistaDevolucao = @DataPrevistaDevolucao,
                    DataRealDevolucao = @DataRealDevolucao,
                    Status = @Status
                WHERE Id = @Id;
            ";

            await connection.ExecuteAsync(sql, emprestimo);
        }

        public async Task<IEnumerable<Emprestimo>> GetAtrasadosAsync()
        {
            using var connection = new SqliteConnection(_connectionString);

            const string sql = @"
                SELECT * FROM Emprestimo
                WHERE Status = 'ATRASADO';
            ";

            return await connection.QueryAsync<Emprestimo>(sql);
        }

        public async Task<IEnumerable<Emprestimo>> GetByUsuarioAsync(int usuarioId)
        {
            using var connection = new SqliteConnection(_connectionString);

            return await connection.QueryAsync<Emprestimo>(
                "SELECT * FROM Emprestimo WHERE UsuarioId = @usuarioId",
                new { usuarioId }
            );
        }

        public async Task<IEnumerable<Emprestimo>> GetByLivroAsync(string isbn)
        {
            using var connection = new SqliteConnection(_connectionString);

            return await connection.QueryAsync<Emprestimo>(
                "SELECT * FROM Emprestimo WHERE ISBNLivro = @isbn",
                new { isbn }
            );
        }
    }
}
