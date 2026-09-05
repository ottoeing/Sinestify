namespace Sinesify
{
    public class Music
    {
        public string Nome { get; set; }
        public string Cantor { get; set; }
        public Genero? Genero { get; set; }
        public Emoção? Sentimento { get; set; }
        public int Velocidade { get; set; }

        public Music(string nome, string cantor, Genero? genero = null, Emoção? sentimento = null, int velocidade = 0)
        {
            Nome = nome;
            Cantor = cantor;
            Genero = genero;
            Sentimento = sentimento;
            Velocidade = velocidade;
        }
    }
}
