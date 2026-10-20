using Sinesify.Service;

namespace Sinesify
{
    internal class Programa
    {
        static void Main()
        {
            var menu = new Menu(
                new MusicaService(),
                new GeneroService(),
                new EmocaoService());
            menu.Iniciar();
        }
    }
}