namespace Sinesify
{
    public class Genero
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public List<Musica> Musicas { get; set; } = new();

        public Genero() { }

        public Genero(string nome)
        {
            Nome = nome;
        }
    }
}
