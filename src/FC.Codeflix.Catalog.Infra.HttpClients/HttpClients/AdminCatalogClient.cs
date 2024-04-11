using System.Net.Http.Json;
using FC.Codeflix.Catalog.Domain.Entity;
using FC.Codeflix.Catalog.Domain.Gateways;
using FC.Codeflix.Catalog.Domain.Repositories;
using FC.Codeflix.Catalog.Infra.HttpClients.Configuration;
using FC.Codeflix.Catalog.Infra.HttpClients.Models;

namespace FC.Codeflix.Catalog.Infra.HttpClients.HttpClients;

public class AdminCatalogClient : IAdminCatalogGateway
{
    private readonly HttpClient _client;
    private readonly ICastMemberRepository _castMemberRepository;

    public AdminCatalogClient(HttpClient client, ICastMemberRepository castMemberRepository)
    {
        _client = client;
        _castMemberRepository = castMemberRepository;
    }

    public async Task<Genre> GetGenreAsync(Guid id, CancellationToken cancellationToken)
    {
        var response = await _client.GetFromJsonAsync<DataWrapper<GenreOutputModel>>(
            $"genres/{id}", SerializerConfiguration.SnakeCaseSerializerOptions, cancellationToken);
        return response!.Data.ToGenre();
    }

    public async Task<Video> GetVideoAsync(Guid id, CancellationToken cancellationToken)
    {
        var response = await _client.GetFromJsonAsync<DataWrapper<VideoOutputModel>>(
            $"videos/{id}", SerializerConfiguration.SnakeCaseSerializerOptions, cancellationToken);
        var castMembers = await _castMemberRepository
            .GetCastMembersByIdsAsync(response!.Data.CastMembers.Select(x => x.Id), cancellationToken);
        return response!.Data.ToVideo(castMembers.ToArray());
    }
}