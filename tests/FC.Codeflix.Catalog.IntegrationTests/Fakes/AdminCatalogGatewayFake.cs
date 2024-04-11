using FC.Codeflix.Catalog.Domain.Entity;
using FC.Codeflix.Catalog.Domain.Gateways;
using FC.Codeflix.Catalog.Tests.Shared;

namespace FC.Codeflix.Catalog.IntegrationTests.Fakes;

public class AdminCatalogGatewayFake
    : IAdminCatalogGateway
{
    private readonly GenreDataGenerator _genreDataGenerator = new();
    private readonly VideoDataGenerator _videoDataGenerator = new();
    public Task<Domain.Entity.Genre> GetGenreAsync(Guid id, CancellationToken cancellationToken)
    {
        return Task.FromResult(_genreDataGenerator.GetValidGenre(id));
    }

    public Task<Video> GetVideoAsync(Guid id, CancellationToken cancellationToken)
    {
        return Task.FromResult(_videoDataGenerator.GetValidVideo());
    }
}