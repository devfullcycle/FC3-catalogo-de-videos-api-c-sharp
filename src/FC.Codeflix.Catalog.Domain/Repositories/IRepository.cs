using FC.Codeflix.Catalog.Domain.Repositories.DTOs;

namespace FC.Codeflix.Catalog.Domain.Repositories;
public interface IRepository<T> where T : class
{
    Task SaveAsync(T entity, CancellationToken cancellationToken);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    Task<SearchOutput<T>> SearchAsync(
        SearchInput input,
        CancellationToken cancellationToken
    );
}
