namespace Sinesify
{
    public class Emoção
    {
        public int Id { get; set; }
        public string Sentimento { get; set; } = string.Empty;
        public List<Music> Musicas { get; set; } = new();

        public Emoção() { }

        public Emoção(string sentimento)
        {
            Sentimento = sentimento;
        }
    }
}
