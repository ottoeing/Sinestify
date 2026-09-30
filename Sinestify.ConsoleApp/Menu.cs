using Sinesify.Service;

namespace Sinesify
{
    public class Menu
    {
        private readonly MusicaService musicaService;
        private readonly GeneroService generoService;
        private readonly EmocaoService emocaoService;
        private readonly Tocador tocador;

        public Menu(MusicaService musicaService, GeneroService generoService, EmocaoService emocaoService)
        {
            this.musicaService = musicaService;
            this.generoService = generoService;
            this.emocaoService = emocaoService;
            tocador = new Tocador();
        }

        public async Task IniciarAsync()
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
                Console.WriteLine();
                Console.Write("Escolha: ");

                string? opcao = Console.ReadLine();

                switch (opcao)
                {
                    case "1": await ListarMusicasAsync(); break;
                    case "2": await CadastrarMusicaAsync(); break;
                    case "3": await EditarMusicaAsync(); break;
                    case "4": await ExcluirMusicaAsync(); break;
                    case "5": await TocarMusicaAsync(); break;
                    case "6": return;
                    default:
                        Avisar("Opção inválida.");
                        break;
                }
            }
        }

        private async Task ListarMusicasAsync(bool pausar = true)
        {
            Console.Clear();
            var resultado = await musicaService.ListarAsync();
            if (!resultado.Sucesso)
            {
                Avisar(resultado.Mensagem);
                return;
            }

            var musicas = resultado.Valor!;
            Console.WriteLine("========== LISTA DE MÚSICAS ==========");
            foreach (var musica in musicas)
            {
                Console.WriteLine($"ID {musica.Id} - {musica.Nome} - {musica.Cantor} [{musica.Genero?.Nome}] ({musica.Emocao?.Sentimento}, {musica.Velocidade} BPM)");
            }

            if (musicas.Count == 0)
                Console.WriteLine("Nenhuma música cadastrada.");
            if (pausar)
                Pausar();
        }

        private async Task CadastrarMusicaAsync()
        {
            Console.Clear();
            Console.WriteLine("========== NOVA MÚSICA ==========");
            var resultado = await musicaService.AdicionarAsync(await LerDadosMusicaAsync());
            Avisar(resultado.Mensagem);
        }

        private async Task EditarMusicaAsync()
        {
            await ListarMusicasAsync(false);
            int id = LerInteiro("\nID da música");
            var resultado = await musicaService.ObterPorIdAsync(id);
            if (!resultado.Sucesso)
            {
                Avisar(resultado.Mensagem);
                return;
            }

            var musica = resultado.Valor!;
            musica.Nome = LerTexto("Nome", musica.Nome);
            musica.Cantor = LerTexto("Cantor", musica.Cantor);
            musica.GeneroId = await EscolherGeneroAsync(musica.GeneroId);
            musica.EmocaoId = await EscolherEmocaoAsync(musica.EmocaoId);
            musica.Velocidade = LerInteiro("Velocidade BPM", musica.Velocidade);
            resultado = await musicaService.AtualizarAsync(musica);
            Avisar(resultado.Mensagem);
        }

        private async Task ExcluirMusicaAsync()
        {
            await ListarMusicasAsync(false);
            int id = LerInteiro("\nID da música");
            var resultado = await musicaService.ObterPorIdAsync(id);
            if (!resultado.Sucesso)
            {
                Avisar(resultado.Mensagem);
                return;
            }

            var musica = resultado.Valor!;
            Console.Write($"Excluir \"{musica.Nome}\"? (s/n): ");
            if (Console.ReadLine()?.Trim().Equals("s", StringComparison.OrdinalIgnoreCase) == true)
            {
                var resultadoExclusao = await musicaService.ExcluirAsync(id);
                Avisar(resultadoExclusao.Mensagem);
            }
        }

        private async Task TocarMusicaAsync()
        {
            await ListarMusicasAsync(false);
            int id = LerInteiro("\nID da música");
            var resultado = await musicaService.ObterPorIdAsync(id);
            if (!resultado.Sucesso)
            {
                Avisar(resultado.Mensagem);
                return;
            }

            tocador.Tocar(resultado.Valor!);
        }

        private async Task<Musica> LerDadosMusicaAsync()
        {
            string nome = LerTexto("Nome");
            string cantor = LerTexto("Cantor");
            return new Musica(nome, cantor)
            {
                GeneroId = await EscolherGeneroAsync(),
                EmocaoId = await EscolherEmocaoAsync(),
                Velocidade = LerInteiro("Velocidade BPM")
            };
        }

        private async Task<int> EscolherGeneroAsync(int atual = 0)
        {
            var resultado = await generoService.ListarAsync();
            if (!resultado.Sucesso)
            {
                Console.WriteLine(resultado.Mensagem);
                return 0;
            }

            var generos = resultado.Valor!;
            foreach (var genero in generos)
                Console.WriteLine($"{genero.Id} - {genero.Nome}");
            return EscolherId("Gênero", generos.Select(g => g.Id).ToHashSet(), atual);
        }

        private async Task<int> EscolherEmocaoAsync(int atual = 0)
        {
            var resultado = await emocaoService.ListarAsync();
            if (!resultado.Sucesso)
            {
                Console.WriteLine(resultado.Mensagem);
                return 0;
            }

            var emocoes = resultado.Valor!;
            foreach (var emocao in emocoes)
                Console.WriteLine($"{emocao.Id} - {emocao.Sentimento}");
            return EscolherId("Emoção", emocoes.Select(e => e.Id).ToHashSet(), atual);
        }

        private static int EscolherId(string campo, HashSet<int> ids, int atual)
        {
            while (true)
            {
                int valor = LerInteiro(campo, atual);
                if (ids.Contains(valor))
                    return valor;
                Console.WriteLine($"{campo} inválido. Escolha uma opção da lista.");
            }
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
                Console.Write($"{campo}{(atual > 0 ? $" [{atual}]" : string.Empty)}: ");
                string valor = Console.ReadLine()?.Trim() ?? string.Empty;
                if (string.IsNullOrEmpty(valor) && atual > 0)
                    return atual;
                if (int.TryParse(valor, out int resultado) && resultado >= 0)
                    return resultado;
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
