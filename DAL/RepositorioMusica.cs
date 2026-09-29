using Microsoft.EntityFrameworkCore;

namespace Sinesify.DAL
{
    public class RepositorioMusica
    {
        public List<Music> Listar()
        {
            using var contexto = new SinestifyContext();
            return contexto.Musicas
                .Include(music => music.Genero)
                .Include(music => music.Sentimento)
                .AsNoTracking()
                .OrderBy(music => music.MusicId)
                .ToList();
        }

        public Music? ObterPorId(int id)
        {
            using var contexto = new SinestifyContext();
            return contexto.Musicas
                .Include(music => music.Genero)
                .Include(music => music.Sentimento)
                .AsNoTracking()
                .SingleOrDefault(music => music.MusicId == id);
        }

        public Music Adicionar(Music music)
        {
            using var contexto = new SinestifyContext();
            contexto.Musicas.Add(music);
            contexto.SaveChanges();
            return ObterPorId(music.MusicId)!;
        }

        public bool Atualizar(Music music)
        {
            using var contexto = new SinestifyContext();
            var musicPersistida = contexto.Musicas.SingleOrDefault(item => item.MusicId == music.MusicId);
            if (musicPersistida is null)
                return false;

            musicPersistida.Nome = music.Nome;
            musicPersistida.Cantor = music.Cantor;
            musicPersistida.GeneroId = music.GeneroId;
            musicPersistida.EmocaoId = music.EmocaoId;
            musicPersistida.Velocidade = music.Velocidade;
            contexto.SaveChanges();
            return true;
        }

        public bool Excluir(int id)
        {
            using var contexto = new SinestifyContext();
            var music = contexto.Musicas.Find(id);
            if (music is null)
                return false;

            contexto.Musicas.Remove(music);
            contexto.SaveChanges();
            return true;
        }
    }
}