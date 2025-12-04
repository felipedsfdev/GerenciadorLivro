namespace Domain
{
    public class Multa
    {
        public int IdEmprestimo { get; set; }
        public decimal Valor { get; set; }
        public string Status { get; set; } = string.Empty;       // PENDENTE, PAGA
    }
}
