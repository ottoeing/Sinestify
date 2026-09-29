using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Sinesify.Business;
using Sinesify.Service;

namespace Sinesify
{
    internal class Programa
    {
        static async Task Main(string[] args)
        {
            var servicos = new ServiceCollection();

            servicos.AddDbContext<ContextoSinestify>();
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