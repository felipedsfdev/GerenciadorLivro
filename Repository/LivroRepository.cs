using Dapper;
using Domain;
using Microsoft.Data.Sqlite;

namespace Repository
{
    public class LivroRepository : ILivroRepository
    {
        private readonly string _connectionString;

        public LivroRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<Livro>> GetAllAsync()
        {
            using var connection = new SqliteConnection(_connectionString);
            return await connection.QueryAsync<Livro>("SELECT * FROM Livro");
        }

        public async Task<Livro?> GetByIsbnAsync(string isbn)
        {
            using var connection = new SqliteConnection(_connectionString);

            return await connection.QuerySingleOrDefaultAsync<Livro>(
                "SELECT * FROM Livro WHERE ISBN = @isbn",
                new { isbn }
            );
        }

        public async Task AddAsync(Livro livro)
        {
            using var connection = new SqliteConnection(_connectionString);

            const string sql = @"
                INSERT INTO Livro (ISBN, Titulo, Autor, Categoria, Status, DataCadastro)
                VALUES (@ISBN, @Titulo, @Autor, @Categoria, @Status, @DataCadastro);
            ";

            await connection.ExecuteAsync(sql, livro);
        }

        public async Task UpdateAsync(Livro livro)
        {
            using var connection = new SqliteConnection(_connectionString);

            const string sql = @"
                UPDATE Livro SET 
                    Titulo = @Titulo,
                    Autor = @Autor,
                    Categoria = @Categoria,
                    Status = @Status
                WHERE ISBN = @ISBN;
            ";

            await connection.ExecuteAsync(sql, livro);
        }

        public async Task DeleteAsync(string isbn)
        {
            using var connection = new SqliteConnection(_connectionString);

            await connection.ExecuteAsync(
                "DELETE FROM Livro WHERE ISBN = @isbn",
                new { isbn }
            );
        }
    }
}
