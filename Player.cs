using System;
using System.Threading;

namespace Sinesify
{
    public class Player
    {
        private Random random = new Random();

        public void Executar(Music music)
        {
            Console.CursorVisible = false;
            double time = 0;

            try
            {
                while (!Console.KeyAvailable)
                {
                    Console.SetCursorPosition(0, 0);
                    Console.Clear();

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Tocando: {music.Nome}");
                    Console.WriteLine($"Artista: {music.Cantor}");
                    Console.ResetColor();
                    Console.WriteLine();

                    const int columns = 15;
                    const int maxHeight = 12;
                    ConsoleColor waveColor = ObterCorPorSentimento(music);

                    for (int row = maxHeight; row >= 1; row--)
                    {
                        Console.ForegroundColor = waveColor;
                        for (int col = 0; col < columns; col++)
                        {
                            double value =
                                Math.Sin(time + col * 0.35) * Math.Sin(time * 0.6) +
                                Math.Sin(time * 0.4 + col * 0.2) * Math.Cos(time * 0.3) +
                                Math.Sin(time * 0.8 + col * 0.5) * 0.5;

                            value += (random.NextDouble() - 0.5) * 0.3;

                            int height = (int)((value + 1.5) * (maxHeight / 3.0));
                            height = Math.Max(0, Math.Min(maxHeight, height));

                            Console.Write(height >= row ? "█" : " ");
                        }

                        Console.ResetColor();
                        Console.WriteLine();
                    }

                    Console.WriteLine();
                    Console.WriteLine("Pressione qualquer tecla para voltar...");

                    time += ObterIncrementoPorVelocidade(music.Velocidade);
                    Thread.Sleep(ObterDelayPorVelocidade(music.Velocidade));
                }

                Console.ReadKey(true);
            }
            finally
            {
                Console.CursorVisible = true;
            }
        }

        private ConsoleColor ObterCorPorSentimento(Music music)
        {
            string sentimento = music.Sentimento?.Sentimento?.ToLowerInvariant() ?? string.Empty;
            return sentimento switch
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

        private int ObterDelayPorVelocidade(int velocidade)
        {
            int vel = Math.Max(1, Math.Min(200, velocidade));
            return 100 - (int)((vel / 200.0) * 60);
        }
    }
}