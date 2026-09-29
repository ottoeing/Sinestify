using Sinesify.DAL;

namespace Sinesify
{
    public class Menu
    {
        private readonly RepositorioMusica repositorio;
        private readonly Player player;

        public Menu()
        {
            repositorio = new RepositorioMusica();
            player = new Player();
            InicializarBanco();
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
                Console.WriteLine("1 - Listar músicas");
                Console.WriteLine("2 - Cadastrar música");
                Console.WriteLine("3 - Editar música");
                Console.WriteLine("4 - Excluir música");
                Console.WriteLine("5 - Tocar música");
                Console.WriteLine("6 - Sair");
                Console.Write("\nEscolha: ");

                switch (Console.ReadLine())
                {
                    case "1": ListarMusicas(); break;
                    case "2": CriarMusica(); break;
                    case "3": EditarMusica(); break;
                    case "4": ExcluirMusica(); break;
                    case "5": TocarMusica(); break;
                    case "6": return;
                    default: Avisar("Opção inválida."); break;
                }
            }
        }

        private void ListarMusicas(bool pausar = true)
        {
            Console.Clear();
            Console.WriteLine("========== LISTA DE MÚSICAS ==========\n");
            var musicas = repositorio.Listar();
            if (musicas.Count == 0)
            {
                Console.WriteLine("Nenhuma música cadastrada.");
            }
            else
            {
                foreach (var music in musicas)
                    ExibirMusica(music);
            }

            if (pausar)
                Pausar();
        }

        private void CriarMusica()
        {
            Console.Clear();
            Console.WriteLine("========== NOVA MÚSICA ==========\n");
            var music = LerDadosDaMusica();
            repositorio.Adicionar(music);
            Avisar("Música cadastrada com sucesso.");
        }

        private void EditarMusica()
        {
            ListarMusicas(false);
            int id = LerInteiro("\nID da música: ");
            var music = repositorio.ObterPorId(id);
            if (music is null)
            {
                Avisar("Música não encontrada.");
                return;
            }

            Console.WriteLine("\nDeixe o campo vazio para manter o valor atual.");
            music.Nome = LerTexto("Nome", music.Nome);
            music.Cantor = LerTexto("Cantor", music.Cantor);
            music.GeneroId = EscolherGenero(music.GeneroId);
            music.EmocaoId = EscolherEmocao(music.EmocaoId);
            music.Velocidade = LerInteiro("Velocidade BPM", music.Velocidade);
            repositorio.Atualizar(music);
            Avisar("Música atualizada com sucesso.");
        }

        private void ExcluirMusica()
        {
            ListarMusicas(false);
            int id = LerInteiro("\nID da música: ");
            var music = repositorio.ObterPorId(id);
            if (music is null)
            {
                Avisar("Música não encontrada.");
                return;
            }

            Console.Write($"Excluir \"{music.Nome}\"? (s/n): ");
            if (Console.ReadLine()?.Trim().ToLowerInvariant() == "s")
            {
                repositorio.Excluir(id);
                Avisar("Música excluída com sucesso.");
            }
        }

        private void TocarMusica()
        {
            ListarMusicas(false);
            int id = LerInteiro("\nID da música: ");
            var music = repositorio.ObterPorId(id);
            if (music is null)
            {
                Avisar("Música não encontrada.");
                return;
            }

            player.Executar(music);
        }

        private Music LerDadosDaMusica()
        {
            string nome = LerTexto("Nome");
            string cantor = LerTexto("Cantor");
            int generoId = EscolherGenero();
            int emocaoId = EscolherEmocao();
            int velocidade = LerInteiro("Velocidade BPM");
            return new Music(nome, cantor)
            {
                GeneroId = generoId,
                EmocaoId = emocaoId,
                Velocidade = velocidade
            };
        }

        private int EscolherGenero(int atual = 0)
        {
            using var contexto = new SinestifyContext();
            var generos = contexto.Generos.OrderBy(genero => genero.GeneroId).ToList();
            foreach (var genero in generos)
                Console.WriteLine($"{genero.GeneroId} - {genero.Nome}");
            return LerInteiro($"Gênero{(atual > 0 ? $" [{atual}]" : string.Empty)}", atual);
        }

        private int EscolherEmocao(int atual = 0)
        {
            using var contexto = new SinestifyContext();
            var emocoes = contexto.Emocoes.OrderBy(emocao => emocao.EmocaoId).ToList();
            foreach (var emocao in emocoes)
                Console.WriteLine($"{emocao.EmocaoId} - {emocao.Sentimento}");
            return LerInteiro($"Emoção{(atual > 0 ? $" [{atual}]" : string.Empty)}", atual);
        }

        private void InicializarBanco()
        {
            using var contexto = new SinestifyContext();
            contexto.Database.EnsureCreated();
            if (contexto.Generos.Any() || contexto.Emocoes.Any() || contexto.Musicas.Any())
                return;

            var generos = new[] { "Pop", "Rock", "Eletrônica", "R&B", "Clássica", "Hip Hop", "Jazz", "Reggae", "Funk", "Country", "Indie", "Metal" }
                .Select(nome => new Genero(nome)).ToList();
            var emocoes = new[] { "Alegria", "Tristeza", "Euforia", "Saudade", "Calma", "Nostalgia", "Esperança", "Melancolia", "Empolgação", "Romantismo" }
                .Select(sentimento => new Emoção(sentimento)).ToList();
            contexto.Generos.AddRange(generos);
            contexto.Emocoes.AddRange(emocoes);
            contexto.SaveChanges();

            contexto.Musicas.AddRange(
                new Music("Believer", "Imagine Dragons", generos[1], emocoes[2], 120),
                new Music("Shape Of You", "Ed Sheeran", generos[0], emocoes[0], 76),
                new Music("Blinding Lights", "The Weeknd", generos[2], emocoes[3], 65),
                new Music("Numb", "Linkin Park", generos[11], emocoes[1], 90),
                new Music("Viva La Vida", "Coldplay", generos[10], emocoes[0], 110));
            contexto.SaveChanges();
        }

        private static void ExibirMusica(Music music)
        {
            Console.WriteLine($"ID {music.MusicId} - {music.Nome} - {music.Cantor} [{music.Genero?.Nome}] ({music.Sentimento?.Sentimento}, {music.Velocidade} BPM)");
        }

        private static string LerTexto(string campo, string atual = "")
        {
            Console.Write($"{campo}{(string.IsNullOrEmpty(atual) ? string.Empty : $" [{atual}]")}: ");
            string valor = Console.ReadLine()?.Trim() ?? string.Empty;
            return string.IsNullOrEmpty(valor) ? atual : valor;
        }

        private static int LerInteiro(string campo, int atual = 0)
        {
            while (true)
            {
                Console.Write($"{campo}: ");
                if (int.TryParse(Console.ReadLine(), out int valor) && valor >= 0)
                    return valor;
                if (atual > 0)
                    return atual;
                Console.WriteLine("Informe um número válido.");
            }
        }

        private static void Avisar(string mensagem)
        {
            Console.WriteLine($"\n{mensagem}");
            Pausar();
        }

        private static void Pausar()
        {
            Console.WriteLine("Pressione Enter para continuar...");
            Console.ReadLine();
        }
    }
}
