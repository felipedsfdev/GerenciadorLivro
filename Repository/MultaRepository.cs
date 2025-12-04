using Dapper;
using Domain;
using Microsoft.Data.Sqlite;

namespace Repository
{
    public class MultaRepository : IMultaRepository
    {
        private readonly string _connectionString;

        public MultaRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<Multa?> GetByEmprestimoIdAsync(int emprestimoId)
        {
            using var connection = new SqliteConnection(_connectionString);

            return await connection.QuerySingleOrDefaultAsync<Multa>(
                "SELECT * FROM Multa WHERE EmprestimoId = @emprestimoId",
                new { emprestimoId }
            );
        }

        public async Task AddAsync(Multa multa)
        {
            using var connection = new SqliteConnection(_connectionString);

            const string sql = @"
                INSERT INTO Multa (EmprestimoId, Valor, Status)
                VALUES (@EmprestimoId, @Valor, @Status);
            ";

            await connection.ExecuteAsync(sql, multa);
        }

        public async Task UpdateAsync(Multa multa)
        {
            using var connection = new SqliteConnection(_connectionString);

            const string sql = @"
                UPDATE Multa
                SET Valor = @Valor,
                    Status = @Status
                WHERE EmprestimoId = @EmprestimoId;
            ";

            await connection.ExecuteAsync(sql, multa);
        }
    }
}
