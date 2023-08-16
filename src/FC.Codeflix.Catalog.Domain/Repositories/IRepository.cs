using FC.Codeflix.Catalog.Domain.Repositories.DTOs;

namespace FC.Codeflix.Catalog.Domain.Repositories;
public interface IRepository<T> where T : class
{
    Task SaveAsync(T entity, CancellationToken cancellationToken);
    Task<T> GetAsync(Guid id, CancellationToken cancellationToken);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    Task<SearchOutput<T>> Search(
        SearchInput input,
        CancellationToken cancellationToken
    );
}
