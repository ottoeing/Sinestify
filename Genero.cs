namespace Sinesify
{
    public class Genero
    {
        public int GeneroId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public ICollection<Music> Musicas { get; set; } = new List<Music>();

        public Genero()
        {
        }

        public Genero(string nome)
        {
            Nome = nome;
        }
    }
}
