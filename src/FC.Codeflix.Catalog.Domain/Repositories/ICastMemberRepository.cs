using FC.Codeflix.Catalog.Domain.Entity;

namespace FC.Codeflix.Catalog.Domain.Repositories;

public interface ICastMemberRepository : IRepository<CastMember>
{
    Task<IEnumerable<CastMember>> GetCastMembersByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken);
}