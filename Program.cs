using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Sinesify
{
    internal class Programa
    {
        static void Main(string[] args)
        {
            var servicos = new ServiceCollection();

            servicos.AddDbContext<ContextoSinestify>();
            servicos.AddScoped<RepositorioMusicas>();
            servicos.AddScoped<Menu>();

            using var provedorServicos = servicos.BuildServiceProvider();
            Menu menu = provedorServicos.GetRequiredService<Menu>();
            menu.Iniciar();
        }
    }
}