using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Sinesify
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();

            services.AddDbContext<SinestifyContext>();
            services.AddScoped<MusicDAO>();
            services.AddScoped<Menu>();

            using var serviceProvider = services.BuildServiceProvider();
            Menu menu = serviceProvider.GetRequiredService<Menu>();
            menu.Iniciar();
        }
    }
}