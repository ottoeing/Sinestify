namespace Sinesify
{
    public class Musica
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Cantor { get; set; } = string.Empty;
        public int GeneroId { get; set; }
        public Genero? Genero { get; set; }
        public int EmocaoId { get; set; }
        public Emocao? Emocao { get; set; }
        public int Velocidade { get; set; }

        public Musica() { }

        public Musica(string nome, string cantor, Genero? genero = null, Emocao? emocao = null, int velocidade = 0)
        {
            Nome = nome;
            Cantor = cantor;
            Genero = genero;
            Emocao = emocao;
            Velocidade = velocidade;
        }
    }
}
