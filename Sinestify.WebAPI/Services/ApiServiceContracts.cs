using Sinestify.WebAPI.Dtos;

namespace Sinestify.WebAPI.Services;

public interface IMusicaApiService
{
    IReadOnlyList<MusicaResponse> GetAll();
    MusicaResponse? GetById(int id);
    ApiOperationResult<MusicaResponse> Criar(CreateMusicaRequest request);
    ApiOperationResult<MusicaResponse> Atualizar(int id, UpdateMusicaRequest request);
    bool Excluir(int id);
}

public interface IGeneroApiService
{
    IReadOnlyList<GeneroResponse> GetAll();
}

public interface IEmocaoApiService
{
    IReadOnlyList<EmocaoResponse> GetAll();
}