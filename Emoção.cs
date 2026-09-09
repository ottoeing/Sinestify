namespace Sinesify
{
    public class Emoção
    {
        public int EmocaoId { get; set; }
        public string Sentimento { get; set; } = string.Empty;
        public ICollection<Music> Musicas { get; set; } = new List<Music>();

        public Emoção()
        {
        }

        public Emoção(string sentimento)
        {
            Sentimento = sentimento;
        }
    }
}
