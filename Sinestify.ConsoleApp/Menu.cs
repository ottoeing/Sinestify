using Sinesify.Service;

namespace Sinesify
{
    public class Menu
    {
        private readonly MusicaService musicaService;
        private readonly GeneroService generoService;
        private readonly EmocaoService emocaoService;
        private readonly UsuarioService usuarioService;
        private readonly Tocador tocador;
        private Usuario? usuarioAtual;
        private bool rodando = true;

        public Menu(MusicaService musicaService, GeneroService generoService, EmocaoService emocaoService)
        {
            this.musicaService = musicaService;
            this.generoService = generoService;
            this.emocaoService = emocaoService;
            usuarioService = new UsuarioService();
            tocador = new Tocador();
        }

        public void Iniciar()
        {
            while (rodando)
            {
                if (usuarioAtual is null)
                {
                    MostrarTelaAutenticacao();
                    continue;
                }

                MostrarMenuPrincipal();
            }
        }

        private void MostrarTelaAutenticacao()
        {
            Console.Clear();
            Console.WriteLine("=======================================");
            Console.WriteLine("          ACESSO AO SINESTIFY");
            Console.WriteLine("=======================================");
            Console.WriteLine();
            Console.WriteLine("1 - Registrar usuário");
            Console.WriteLine("2 - Fazer login");
            Console.WriteLine("0 - Sair");
            Console.Write("\nEscolha uma opção: ");

            switch (Console.ReadLine())
            {
                case "1":
                    RegistrarUsuario();
                    break;
                case "2":
                    FazerLogin();
                    break;
                case "0":
                    rodando = false;
                    break;
                default:
                    Avisar("Opção inválida.");
                    break;
            }
        }

        private void RegistrarUsuario()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("========== REGISTRAR USUÁRIO ==========\n");

                var nome = LerTexto("Nome");
                var email = LerTexto("E-mail");
                Console.Write("Senha: ");
                var senha = Console.ReadLine() ?? string.Empty;

                var usuarioCriado = usuarioService.Registrar(new Usuario
                {
                    Nome = nome,
                    Email = email,
                    Senha = senha
                });

                if (usuarioCriado is null)
                {
                    Avisar("Usuário já existe ou os dados são inválidos.");
                    continue;
                }

                usuarioAtual = usuarioCriado;
                Avisar($"Usuário '{usuarioCriado.Nome}' registrado com sucesso!");
                return;
            }
        }

        private void FazerLogin()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("========== LOGIN ==========\n");

                var email = LerTexto("E-mail");
                Console.Write("Senha: ");
                var senha = Console.ReadLine() ?? string.Empty;

                var usuario = usuarioService.Login(new Usuario
                {
                    Email = email,
                    Senha = senha
                });

                if (usuario is null)
                {
                    Avisar("Credenciais inválidas.");
                    continue;
                }

                usuarioAtual = usuario;
                Avisar($"Bem-vindo, {usuario.Nome}!");
                return;
            }
        }

        private void MostrarMenuPrincipal()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("=======================================");
            Console.WriteLine("               SINESTIFY");
            Console.WriteLine("=======================================");
            Console.ResetColor();
            Console.WriteLine($"Usuário: {usuarioAtual!.Nome}");
            Console.WriteLine();
            Console.WriteLine("1 - Listar músicas");
            Console.WriteLine("2 - Cadastrar música");
            Console.WriteLine("3 - Editar música");
            Console.WriteLine("4 - Excluir música");
            Console.WriteLine("5 - Tocar música");
            Console.WriteLine("0 - Sair da conta");
            Console.WriteLine("Pressione Enter vazio para sair do aplicativo.");
            Console.Write("\nEscolha: ");

            var opcao = Console.ReadLine();
            if (string.IsNullOrEmpty(opcao))
            {
                rodando = false;
                return;
            }

            switch (opcao)
            {
                case "1": ListarMusicas(); break;
                case "2": CadastrarMusica(); break;
                case "3": EditarMusica(); break;
                case "4": ExcluirMusica(); break;
                case "5": TocarMusica(); break;
                case "0":
                    usuarioAtual = null;
                    Avisar("Conta desconectada.");
                    break;
                default:
                    Avisar("Opção inválida.");
                    break;
            }
        }

        private void ListarMusicas(bool pausar = true)
        {
            Console.Clear();
            var musicas = musicaService.Listar();
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
            var musica = musicaService.Adicionar(LerDadosMusica());
            Avisar(musica is null ? "Não foi possível cadastrar a música." : "Música cadastrada com sucesso.");
        }

        private void EditarMusica()
        {
            ListarMusicas(false);
            int id = LerInteiro("\nID da música");
            var musica = musicaService.ObterPorId(id);
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
            var musicaAtualizada = musicaService.Atualizar(musica);
            Avisar(musicaAtualizada is null ? "Não foi possível atualizar a música." : "Música atualizada com sucesso.");
        }

        private void ExcluirMusica()
        {
            ListarMusicas(false);
            int id = LerInteiro("\nID da música");
            var musica = musicaService.ObterPorId(id);
            if (musica is null)
            {
                Avisar("Música não encontrada.");
                return;
            }

            Console.Write($"Excluir \"{musica.Nome}\"? (s/n): ");
            if (Console.ReadLine()?.Trim().Equals("s", StringComparison.OrdinalIgnoreCase) == true)
            {
                Avisar(musicaService.Excluir(id)
                    ? "Música excluída com sucesso."
                    : "Não foi possível excluir a música.");
            }
        }

        private void TocarMusica()
        {
            ListarMusicas(false);
            int id = LerInteiro("\nID da música");
            var musica = musicaService.ObterPorId(id);
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
            var generos = generoService.Listar();
            foreach (var genero in generos)
                Console.WriteLine($"{genero.Id} - {genero.Nome}");
            return EscolherId("Gênero", generos.Select(g => g.Id).ToHashSet(), atual);
        }

        private int EscolherEmocao(int atual = 0)
        {
            var emocoes = emocaoService.Listar();
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
