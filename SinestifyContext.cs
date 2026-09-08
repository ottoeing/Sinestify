using Microsoft.EntityFrameworkCore;

namespace Sinesify;

public class SinestifyContext : DbContext
{
    private const string StringConexao = "Server=127.0.0.1;Port=3306;Database=sinestify;Uid=root;Pwd=1234;SslMode=None;AllowPublicKeyRetrieval=True;ConnectionTimeout=30;";

    public DbSet<Music> Musicas => Set<Music>();
    public DbSet<Genero> Generos => Set<Genero>();
    public DbSet<Emoção> Emocoes => Set<Emoção>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseMySql(StringConexao, ServerVersion.AutoDetect(StringConexao));
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Music>()
            .HasOne(m => m.Genero)
            .WithMany(g => g.Musicas)
            .HasForeignKey(m => m.GeneroId);

        modelBuilder.Entity<Music>()
            .HasOne(m => m.Sentimento)
            .WithMany(e => e.Musicas)
            .HasForeignKey(m => m.SentimentoId);

        modelBuilder.Entity<Genero>().HasData(
            new { Id = 1, Nome = "Pop" }, new { Id = 2, Nome = "Rock" },
            new { Id = 3, Nome = "Eletrônica" }, new { Id = 4, Nome = "R&B" },
            new { Id = 5, Nome = "Clássica" }, new { Id = 6, Nome = "Hip Hop" },
            new { Id = 7, Nome = "Jazz" }, new { Id = 8, Nome = "Reggae" },
            new { Id = 9, Nome = "Funk" }, new { Id = 10, Nome = "Country" },
            new { Id = 11, Nome = "Indie" }, new { Id = 12, Nome = "Metal" });

        modelBuilder.Entity<Emoção>().HasData(
            new { Id = 1, Sentimento = "Alegria" }, new { Id = 2, Sentimento = "Tristeza" },
            new { Id = 3, Sentimento = "Euforia" }, new { Id = 4, Sentimento = "Saudade" },
            new { Id = 5, Sentimento = "Calma" }, new { Id = 6, Sentimento = "Nostalgia" },
            new { Id = 7, Sentimento = "Esperança" }, new { Id = 8, Sentimento = "Melancolia" },
            new { Id = 9, Sentimento = "Empolgação" }, new { Id = 10, Sentimento = "Romantismo" });

        modelBuilder.Entity<Music>().HasData(
            new { Id = 1, Nome = "Believer", Cantor = "Imagine Dragons", GeneroId = 2, SentimentoId = 3, Velocidade = 120 },
            new { Id = 2, Nome = "Thunder", Cantor = "Imagine Dragons", GeneroId = 2, SentimentoId = 3, Velocidade = 118 },
            new { Id = 3, Nome = "Shape Of You", Cantor = "Ed Sheeran", GeneroId = 1, SentimentoId = 1, Velocidade = 76 },
            new { Id = 4, Nome = "Counting Stars", Cantor = "OneRepublic", GeneroId = 11, SentimentoId = 5, Velocidade = 94 },
            new { Id = 5, Nome = "Perfect", Cantor = "Ed Sheeran", GeneroId = 1, SentimentoId = 1, Velocidade = 48 },
            new { Id = 6, Nome = "Blinding Lights", Cantor = "The Weeknd", GeneroId = 3, SentimentoId = 4, Velocidade = 65 },
            new { Id = 7, Nome = "Someone Like You", Cantor = "Adele", GeneroId = 5, SentimentoId = 2, Velocidade = 40 },
            new { Id = 8, Nome = "Numb", Cantor = "Linkin Park", GeneroId = 12, SentimentoId = 2, Velocidade = 90 },
            new { Id = 9, Nome = "Radioactive", Cantor = "Imagine Dragons", GeneroId = 2, SentimentoId = 4, Velocidade = 100 },
            new { Id = 10, Nome = "Viva La Vida", Cantor = "Coldplay", GeneroId = 11, SentimentoId = 1, Velocidade = 110 },
            new { Id = 11, Nome = "Three Little Birds", Cantor = "Bob Marley", GeneroId = 8, SentimentoId = 1, Velocidade = 76 },
            new { Id = 12, Nome = "Uptown Funk", Cantor = "Mark Ronson feat. Bruno Mars", GeneroId = 9, SentimentoId = 3, Velocidade = 115 },
            new { Id = 13, Nome = "Take Me Home, Country Roads", Cantor = "John Denver", GeneroId = 10, SentimentoId = 1, Velocidade = 82 },
            new { Id = 14, Nome = "Yellow", Cantor = "Coldplay", GeneroId = 1, SentimentoId = 10, Velocidade = 87 },
            new { Id = 15, Nome = "The Scientist", Cantor = "Coldplay", GeneroId = 11, SentimentoId = 8, Velocidade = 73 },
            new { Id = 16, Nome = "Don't Stop Believin'", Cantor = "Journey", GeneroId = 4, SentimentoId = 7, Velocidade = 119 },
            new { Id = 17, Nome = "Summer Of '69", Cantor = "Bryan Adams", GeneroId = 4, SentimentoId = 6, Velocidade = 138 },
            new { Id = 18, Nome = "Can't Stop", Cantor = "Red Hot Chili Peppers", GeneroId = 12, SentimentoId = 9, Velocidade = 91 },
            new { Id = 19, Nome = "Wake Me Up", Cantor = "Avicii", GeneroId = 7, SentimentoId = 7, Velocidade = 124 },
            new { Id = 20, Nome = "Sweet Child O' Mine", Cantor = "Guns N' Roses", GeneroId = 4, SentimentoId = 6, Velocidade = 125 });
    }
}
