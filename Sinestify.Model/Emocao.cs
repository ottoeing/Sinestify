namespace Sinesify
{
    public class Emocao
    {
        public int Id { get; set; }
        public string Sentimento { get; set; } = string.Empty;
        public List<Musica> Musicas { get; set; } = new();

        public Emocao() { }

        public Emocao(string sentimento)
        {
            Sentimento = sentimento;
        }
    }
}
