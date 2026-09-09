namespace Sinesify
{
    public class Music
    {
        public int MusicId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Cantor { get; set; } = string.Empty;
        public int GeneroId { get; set; }
        public Genero? Genero { get; set; }
        public int EmocaoId { get; set; }
        public Emoção? Sentimento { get; set; }
        public int Velocidade { get; set; }

        public Music()
        {
        }

        public Music(string nome, string cantor, Genero? genero = null, Emoção? sentimento = null, int velocidade = 0)
        {
            Nome = nome;
            Cantor = cantor;
            Genero = genero;
            GeneroId = genero?.GeneroId ?? 0;
            Sentimento = sentimento;
            EmocaoId = sentimento?.EmocaoId ?? 0;
            Velocidade = velocidade;
        }
    }
}
