using Dapper;
using Domain;
using Microsoft.Data.Sqlite;

namespace Repository
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly string _connectionString;

        public UsuarioRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<Usuario>> GetAllAsync()
        {
            using var connection = new SqliteConnection(_connectionString);

            return await connection.QueryAsync<Usuario>("SELECT * FROM Usuario");
        }

        public async Task<Usuario?> GetByIdAsync(int id)
        {
            using var connection = new SqliteConnection(_connectionString);

            return await connection.QuerySingleOrDefaultAsync<Usuario>(
                "SELECT * FROM Usuario WHERE Id = @id",
                new { id }
            );
        }

        public async Task AddAsync(Usuario usuario)
        {
            using var connection = new SqliteConnection(_connectionString);

            const string sql = @"
                INSERT INTO Usuario (Nome, Email, Tipo, DataCadastro)
                VALUES (@Nome, @Email, @Tipo, @DataCadastro);
            ";

            await connection.ExecuteAsync(sql, usuario);
        }

        public async Task UpdateAsync(Usuario usuario)
        {
            using var connection = new SqliteConnection(_connectionString);

            const string sql = @"
                UPDATE Usuario SET
                    Nome = @Nome,
                    Email = @Email,
                    Tipo = @Tipo
                WHERE Id = @Id;
            ";

            await connection.ExecuteAsync(sql, usuario);
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = new SqliteConnection(_connectionString);

            await connection.ExecuteAsync(
                "DELETE FROM Usuario WHERE Id = @id",
                new { id }
            );
        }

        public async Task<int> CountEmprestimosAtivosAsync(int usuarioId)
        {
            using var connection = new SqliteConnection(_connectionString);

            const string sql = @"
                SELECT COUNT(*) 
                FROM Emprestimo 
                WHERE UsuarioId = @usuarioId AND Status = 'ATIVO';
            ";

            return await connection.ExecuteScalarAsync<int>(sql, new { usuarioId });
        }
    }
}
