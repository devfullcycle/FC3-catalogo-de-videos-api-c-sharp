using FC.Codeflix.Catalog.Domain.Entity;

namespace FC.Codeflix.Catalog.Domain.Gateways;

public interface IAdminCatalogGateway
{
    Task<Genre> GetGenreAsync(Guid id, CancellationToken cancellationToken);
    Task<Video> GetVideoAsync(Guid id, CancellationToken cancellationToken);
}