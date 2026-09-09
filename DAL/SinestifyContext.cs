using Microsoft.EntityFrameworkCore;

namespace Sinesify.DAL
{
    public class SinestifyContext : DbContext
    {
        public DbSet<Music> Musicas => Set<Music>();
        public DbSet<Genero> Generos => Set<Genero>();
        public DbSet<Emoção> Emocoes => Set<Emoção>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=sinestify.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Music>(entity =>
            {
                entity.HasKey(music => music.MusicId);
                entity.Property(music => music.Nome).IsRequired();
                entity.Property(music => music.Cantor).IsRequired();
                entity.HasOne(music => music.Genero)
                    .WithMany(genero => genero.Musicas)
                    .HasForeignKey(music => music.GeneroId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(music => music.Sentimento)
                    .WithMany(emocao => emocao.Musicas)
                    .HasForeignKey(music => music.EmocaoId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Genero>(entity =>
            {
                entity.HasKey(genero => genero.GeneroId);
                entity.Property(genero => genero.Nome).IsRequired();
            });

            modelBuilder.Entity<Emoção>(entity =>
            {
                entity.HasKey(emocao => emocao.EmocaoId);
                entity.Property(emocao => emocao.Sentimento).IsRequired();
            });
        }
    }
}