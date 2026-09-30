using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sinesify.Business;
using Sinesify.Service;

namespace Sinesify
{
    internal class Programa
    {
        static async Task Main(string[] args)
        {
            var configuracao = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .AddEnvironmentVariables()
                .Build();

            var stringConexao = configuracao.GetConnectionString("Sinestify")
                ?? throw new InvalidOperationException("A connection string 'Sinestify' não foi configurada.");

            var servicos = new ServiceCollection();

            servicos.AddDbContext<ContextoSinestify>(opcoes =>
                opcoes.UseMySql(stringConexao, new MySqlServerVersion(new Version(8, 0, 0))));
            servicos.AddScoped<RepositorioMusicas>();
            servicos.AddScoped<RepositorioGeneros>();
            servicos.AddScoped<RepositorioEmocoes>();
            servicos.AddScoped<MusicaBusiness>();
            servicos.AddScoped<GeneroBusiness>();
            servicos.AddScoped<EmocaoBusiness>();
            servicos.AddScoped<MusicaService>();
            servicos.AddScoped<GeneroService>();
            servicos.AddScoped<EmocaoService>();
            servicos.AddScoped<Menu>();

            using var provedorServicos = servicos.BuildServiceProvider();
            Menu menu = provedorServicos.GetRequiredService<Menu>();
            await menu.IniciarAsync();
        }
    }
}