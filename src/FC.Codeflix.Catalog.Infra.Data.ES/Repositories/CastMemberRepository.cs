using FC.Codeflix.Catalog.Domain.Entity;
using FC.Codeflix.Catalog.Domain.Repositories;
using FC.Codeflix.Catalog.Domain.Repositories.DTOs;
using FC.Codeflix.Catalog.Infra.Data.ES.Models;
using Nest;
using SearchInput = FC.Codeflix.Catalog.Domain.Repositories.DTOs.SearchInput;

namespace FC.Codeflix.Catalog.Infra.Data.ES.Repositories;

public class CastMemberRepository : ICastMemberRepository
{
    private readonly IElasticClient _client;

    public CastMemberRepository(IElasticClient client)
    {
        _client = client;
    }

    public async Task SaveAsync(CastMember entity, CancellationToken cancellationToken)
    {
        var model = CastMemberModel.FromEntity(entity);
        await _client.IndexDocumentAsync(model, cancellationToken);
    }

    public Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<SearchOutput<CastMember>> SearchAsync(SearchInput input, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}