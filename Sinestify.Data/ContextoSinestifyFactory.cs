using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Sinesify;

public class ContextoSinestifyFactory : IDesignTimeDbContextFactory<ContextoSinestify>
{
    public ContextoSinestify CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<ContextoSinestify>()
            .UseMySql(
                ContextoSinestify.CriarStringConexao(),
                new MySqlServerVersion(new Version(8, 0, 0)))
            .Options;

        return new ContextoSinestify(options);
    }
}
