using Microsoft.EntityFrameworkCore;
using Sinesify.Service;

namespace Sinesify
{
    internal class Programa
    {
        static void Main()
        {
            using var contexto = new ContextoSinestify();
            contexto.Database.Migrate();

            var menu = new Menu(
                new MusicaService(),
                new GeneroService(),
                new EmocaoService());
            menu.Iniciar();
        }
    }
}