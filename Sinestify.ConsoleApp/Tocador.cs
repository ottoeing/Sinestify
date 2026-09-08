using System;
using System.Threading;

namespace Sinesify
{
    public class Tocador
    {
        private Random aleatorio = new Random();

        public void Tocar(Musica musica)
        {
            if (musica is null || musica.Id <= 0 || string.IsNullOrWhiteSpace(musica.Nome))
            {
                return;
            }

            Console.CursorVisible = false;
            double tempo = 0;

            try
            {
                while (!Console.KeyAvailable)
                {
                    Console.SetCursorPosition(0, 0);
                    Console.Clear();

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Tocando: {musica.Nome}");
                    Console.WriteLine($"Artista: {musica.Cantor}");
                    Console.ResetColor();
                    Console.WriteLine();

                    const int columns = 15;
                    const int maxHeight = 12;
                    ConsoleColor corOnda = ObterCorPorEmocao(musica);

                    for (int row = maxHeight; row >= 1; row--)
                    {
                        Console.ForegroundColor = corOnda;
                        for (int col = 0; col < columns; col++)
                        {
                            double value =
                                Math.Sin(tempo + col * 0.35) * Math.Sin(tempo * 0.6) +
                                Math.Sin(tempo * 0.4 + col * 0.2) * Math.Cos(tempo * 0.3) +
                                Math.Sin(tempo * 0.8 + col * 0.5) * 0.5;

                            value += (aleatorio.NextDouble() - 0.5) * 0.3;

                            int height = (int)((value + 1.5) * (maxHeight / 3.0));
                            height = Math.Max(0, Math.Min(maxHeight, height));

                            Console.Write(height >= row ? "█" : " ");
                        }

                        Console.ResetColor();
                        Console.WriteLine();
                    }

                    Console.WriteLine();
                    Console.WriteLine("Pressione qualquer tecla para voltar...");

                    tempo += ObterIncrementoPorVelocidade(musica.Velocidade);
                    Thread.Sleep(ObterAtrasoPorVelocidade(musica.Velocidade));
                }

                Console.ReadKey(true);
            }
            finally
            {
                Console.CursorVisible = true;
            }
        }

        private ConsoleColor ObterCorPorEmocao(Musica musica)
        {
            string emocao = musica.Emocao?.Sentimento?.ToLowerInvariant() ?? string.Empty;
            return emocao switch
            {
                "alegria" => ConsoleColor.Yellow,
                "tristeza" => ConsoleColor.Blue,
                "euforia" => ConsoleColor.Magenta,
                "saudade" => ConsoleColor.Green,
                "calma" => ConsoleColor.Cyan,
                "nostalgia" => ConsoleColor.DarkGray,
                "esperança" => ConsoleColor.DarkGreen,
                "melancolia" => ConsoleColor.DarkBlue,
                "empolgação" => ConsoleColor.Red,
                "romantismo" => ConsoleColor.DarkMagenta,
                _ => ConsoleColor.White,
            };
        }

        private double ObterIncrementoPorVelocidade(int velocidade)
        {
            int vel = Math.Max(1, Math.Min(200, velocidade));
            return 0.05 + (vel / 200.0) * 1.00;
        }

        private int ObterAtrasoPorVelocidade(int velocidade)
        {
            int vel = Math.Max(1, Math.Min(200, velocidade));
            return 100 - (int)((vel / 200.0) * 60);
        }
    }
}