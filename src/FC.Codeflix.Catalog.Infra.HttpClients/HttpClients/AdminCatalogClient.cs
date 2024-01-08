using System.Net.Http.Json;
using FC.Codeflix.Catalog.Domain.Entity;
using FC.Codeflix.Catalog.Domain.Gateways;
using FC.Codeflix.Catalog.Infra.HttpClients.Models;

namespace FC.Codeflix.Catalog.Infra.HttpClients.HttpClients;

public class AdminCatalogClient : IAdminCatalogGateway
{
    private readonly HttpClient _client;

    public AdminCatalogClient(HttpClient client)
    {
        _client = client;
    }

    public async Task<Genre> GetGenreAsync(Guid id, CancellationToken cancellationToken)
        => (await _client.GetFromJsonAsync<GenreOutputModel>($"genres/{id}", cancellationToken))!
            .ToGenre();
}