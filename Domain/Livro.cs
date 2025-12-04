namespace Domain
{
    public class Livro
    {
        public string ISBN { get; set; } = string.Empty;  
        public string Titulo { get; set; } = string.Empty;
        public string Autor { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;  
        public string Status { get; set; } = string.Empty;   
        public DateTime DataCadastro { get; set; }
    }
}
