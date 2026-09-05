using System;
using System.Collections.Generic;
using System.Threading;

namespace Sinesify
{
    public class Menu
    {
        private List<Music> musicas;
        private List<Genero> generos;
        private List<Emoção> emocoes;
        private Player player;

        public Menu()
        {
            player = new Player();
            generos = new List<Genero>()
            {
                new Genero("Pop"),
                new Genero("Rock"),
                new Genero("Eletrônica"),
                new Genero("R&B"),
                new Genero("Clássica"),
                new Genero("Hip Hop"),
                new Genero("Jazz"),
                new Genero("Reggae"),
                new Genero("Funk"),
                new Genero("Country"),
                new Genero("Indie"),
                new Genero("Metal")
            };

            emocoes = new List<Emoção>()
{
            new Emoção("Alegria"),
            new Emoção("Tristeza"),
            new Emoção("Euforia"),
            new Emoção("Saudade"),
            new Emoção("Calma"),
            new Emoção("Nostalgia"),
            new Emoção("Esperança"),
            new Emoção("Melancolia"),
            new Emoção("Empolgação"),
            new Emoção("Romantismo")
};

            musicas = new List<Music>()
            {
                new Music("Believer", "Imagine Dragons", generos[1], emocoes[2], 120),
                new Music("Thunder", "Imagine Dragons", generos[1], emocoes[2], 118),
                new Music("Shape Of You", "Ed Sheeran", generos[0], emocoes[0], 76),
                new Music("Counting Stars", "OneRepublic", generos[10], emocoes[4], 94),
                new Music("Perfect", "Ed Sheeran", generos[0], emocoes[0], 48),
                new Music("Blinding Lights", "The Weeknd", generos[2], emocoes[3], 65),
                new Music("Someone Like You", "Adele", generos[4], emocoes[1], 40),
                new Music("Numb", "Linkin Park", generos[11], emocoes[1], 90),
                new Music("Radioactive", "Imagine Dragons", generos[1], emocoes[3], 100),
                new Music("Viva La Vida", "Coldplay", generos[10], emocoes[0], 110),
                new Music("Three Little Birds", "Bob Marley", generos[7], emocoes[0], 76),
                new Music("Uptown Funk", "Mark Ronson feat. Bruno Mars", generos[8], emocoes[2], 115),
                new Music("Take Me Home, Country Roads", "John Denver", generos[9], emocoes[0], 82),
                new Music("Yellow", "Coldplay", generos[0], emocoes[9], 87),             
                new Music("The Scientist", "Coldplay", generos[10], emocoes[7], 73),      
                new Music("Don't Stop Believin'", "Journey", generos[3], emocoes[6], 119),
                new Music("Summer Of '69", "Bryan Adams", generos[3], emocoes[5], 138),   
                new Music("Can't Stop", "Red Hot Chili Peppers", generos[11], emocoes[8], 91), 
                new Music("Wake Me Up", "Avicii", generos[6], emocoes[6], 124),           
                new Music("Sweet Child O' Mine", "Guns N' Roses", generos[3], emocoes[5], 125)
            };
        }

        public void Iniciar()
        {
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("=======================================");
                Console.WriteLine("               SINESTIFY");
                Console.WriteLine("=======================================");
                Console.ResetColor();
                Console.WriteLine();
                Console.WriteLine("1 - Escolher Musica");
                Console.WriteLine("2 - Sair");
                Console.WriteLine();
                Console.Write("Escolha: ");

                string? opcao = Console.ReadLine();

                switch (opcao)
                {
                    case "1":
                        EscolherMusica();
                        break;
                    case "2":
                        return;
                    default:
                        Console.WriteLine("Opção Invalida.");
                        Thread.Sleep(1000);
                        break;
                }
            }
        }

        private void EscolherMusica()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("========== LISTA DE MÚSICAS ==========\n");
                for (int i = 0; i < musicas.Count; i++)
                {
                    string genero = musicas[i].Genero?.Nome ?? "Sem gênero";
                    string sentimento = musicas[i].Sentimento?.Sentimento ?? "Sem sentimento";
                    Console.WriteLine($"{i + 1} - {musicas[i].Nome} - {musicas[i].Cantor} [{genero}] ({sentimento}, {musicas[i].Velocidade} BPM)");
                }
                Console.WriteLine("\n0 - Voltar");
                Console.Write("\nEscolha: ");
                if (int.TryParse(Console.ReadLine(), out int escolha))
                {
                    if (escolha == 0)
                        return;
                    if (escolha >= 1 && escolha <= musicas.Count)
                    {
                        player.Executar(musicas[escolha - 1]);
                    }
                }
            }
        }

    }
}
