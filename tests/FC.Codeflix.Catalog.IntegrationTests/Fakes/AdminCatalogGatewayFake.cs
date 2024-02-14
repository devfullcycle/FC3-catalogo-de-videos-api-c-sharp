using FC.Codeflix.Catalog.Domain.Gateways;
using FC.Codeflix.Catalog.Tests.Shared;

namespace FC.Codeflix.Catalog.IntegrationTests.Fakes;

public class AdminCatalogGatewayFake
    : IAdminCatalogGateway
{
    private readonly GenreDataGenerator _dataGenerator = new();
    public Task<Domain.Entity.Genre> GetGenreAsync(Guid id, CancellationToken cancellationToken)
    {
        return Task.FromResult(_dataGenerator.GetValidGenre(id));
    }
}