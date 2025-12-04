using Microsoft.Data.Sqlite;
using Repository;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Connection string
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                      ?? "Data Source=livraria.db;";

// Criar banco e tabelas automaticamente
using (var connection = new SqliteConnection(connectionString))
{
    connection.Open();

    var cmd = connection.CreateCommand();
    cmd.CommandText = @"
        CREATE TABLE IF NOT EXISTS Livro (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            ISBN TEXT,
            Titulo TEXT,
            Autor TEXT,
            Categoria TEXT,
            Status TEXT,
            DataCadastro TEXT
        );

        CREATE TABLE IF NOT EXISTS Usuario (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Nome TEXT,
            Email TEXT,
            Tipo TEXT,
            DataCadastro TEXT
        );

        CREATE TABLE IF NOT EXISTS Emprestimo (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            ISBNLivro TEXT,
            UsuarioId INTEGER,
            DataEmprestimo TEXT,
            DataPrevistaDevolucao TEXT,
            DataRealDevolucao TEXT,
            Status TEXT
        );

        CREATE TABLE IF NOT EXISTS Multa (
            EmprestimoId INTEGER PRIMARY KEY,
            Valor REAL,
            Status TEXT
        );
    ";
    cmd.ExecuteNonQuery();
}

// Registrar repositórios
builder.Services.AddSingleton<ILivroRepository>(sp => new LivroRepository(connectionString));
builder.Services.AddSingleton<IUsuarioRepository>(sp => new UsuarioRepository(connectionString));
builder.Services.AddSingleton<IEmprestimoRepository>(sp => new EmprestimoRepository(connectionString));
builder.Services.AddSingleton<IMultaRepository>(sp => new MultaRepository(connectionString));

var app = builder.Build();

// Ativar Swagger somente em desenvolvimento
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.Run();
