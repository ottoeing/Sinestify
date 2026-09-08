using System;
using System.Collections.Generic;
using System.Threading;

namespace Sinesify
{
    public class Menu
    {
        private readonly List<Music> musicas;
        private readonly Player player;

        public Menu(MusicDAO dao)
        {
            player = new Player();
            musicas = dao.ListarMusicas();
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
