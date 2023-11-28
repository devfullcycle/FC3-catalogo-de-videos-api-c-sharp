using FC.Codeflix.Catalog.Domain.Entity;

namespace FC.Codeflix.Catalog.Domain.Repositories;

public interface IGenreRepository : IRepository<Genre>
{
    Task<IEnumerable<Genre>> GetGenresByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken);
}