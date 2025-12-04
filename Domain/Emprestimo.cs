namespace Domain
{
    public class Emprestimo
    {
        public int Id { get; set; }
        public string IsbnLivro { get; set; } = string.Empty;  
        public int IdUsuario { get; set; }
        public DateTime DataEmprestimo { get; set; }
        public DateTime DataPrevistaDevolucao { get; set; }
        public DateTime? DataRealDevolucao { get; set; }        // opcional (nullable)
        public string Status { get; set; } = string.Empty;      // ATIVO, FINALIZADO, ATRASADO
    }
}
