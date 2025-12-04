namespace Domain
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;       // ALUNO, PROFESSOR, FUNCIONARIO
        public DateTime DataCadastro { get; set; }
    }
}
