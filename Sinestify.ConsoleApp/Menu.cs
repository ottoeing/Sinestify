namespace Sinesify
{
    public class Menu
    {
        private readonly RepositorioMusicas repositorio;
        private readonly Tocador tocador;

        public Menu(RepositorioMusicas repositorio)
        {
            this.repositorio = repositorio;
            tocador = new Tocador();
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
                Console.WriteLine();
                Console.Write("Escolha: ");

                string? opcao = Console.ReadLine();

                switch (opcao)
                {
                    case "1": ListarMusicas(); break;
                    case "2": CadastrarMusica(); break;
                    case "3": EditarMusica(); break;
                    case "4": ExcluirMusica(); break;
                    case "5": TocarMusica(); break;
                    case "6": return;
                    default:
                        Avisar("Opção inválida.");
                        break;
                }
            }
        }

        private void ListarMusicas(bool pausar = true)
        {
            Console.Clear();
            var musicas = repositorio.ListarMusicas();
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

        private void CadastrarMusica()
        {
            Console.Clear();
            Console.WriteLine("========== NOVA MÚSICA ==========");
            repositorio.Adicionar(LerDadosMusica());
            Avisar("Música cadastrada com sucesso.");
        }

        private void EditarMusica()
        {
            ListarMusicas(false);
            int id = LerInteiro("\nID da música");
            var musica = repositorio.ObterPorId(id);
            if (musica is null)
            {
                Avisar("Música não encontrada.");
                return;
            }

            musica.Nome = LerTexto("Nome", musica.Nome);
            musica.Cantor = LerTexto("Cantor", musica.Cantor);
            musica.GeneroId = EscolherGenero(musica.GeneroId);
            musica.EmocaoId = EscolherEmocao(musica.EmocaoId);
            musica.Velocidade = LerInteiro("Velocidade BPM", musica.Velocidade);
            repositorio.Atualizar(musica);
            Avisar("Música atualizada com sucesso.");
        }

        private void ExcluirMusica()
        {
            ListarMusicas(false);
            int id = LerInteiro("\nID da música");
            var musica = repositorio.ObterPorId(id);
            if (musica is null)
            {
                Avisar("Música não encontrada.");
                return;
            }

            Console.Write($"Excluir \"{musica.Nome}\"? (s/n): ");
            if (Console.ReadLine()?.Trim().Equals("s", StringComparison.OrdinalIgnoreCase) == true)
            {
                repositorio.Excluir(id);
                Avisar("Música excluída com sucesso.");
            }
        }

        private void TocarMusica()
        {
            ListarMusicas(false);
            int id = LerInteiro("\nID da música");
            var musica = repositorio.ObterPorId(id);
            if (musica is null)
            {
                Avisar("Música não encontrada.");
                return;
            }

            tocador.Tocar(musica);
        }

        private Musica LerDadosMusica()
        {
            string nome = LerTexto("Nome");
            string cantor = LerTexto("Cantor");
            return new Musica(nome, cantor)
            {
                GeneroId = EscolherGenero(),
                EmocaoId = EscolherEmocao(),
                Velocidade = LerInteiro("Velocidade BPM")
            };
        }

        private int EscolherGenero(int atual = 0)
        {
            var generos = repositorio.ListarGeneros();
            foreach (var genero in generos)
                Console.WriteLine($"{genero.Id} - {genero.Nome}");
            return EscolherId("Gênero", generos.Select(g => g.Id).ToHashSet(), atual);
        }

        private int EscolherEmocao(int atual = 0)
        {
            var emocoes = repositorio.ListarEmocoes();
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
