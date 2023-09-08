using FC.Codeflix.Catalog.Domain.Entity;
using FC.Codeflix.Catalog.Domain.Repositories;
using FC.Codeflix.Catalog.Domain.Repositories.DTOs;
using FC.Codeflix.Catalog.Infra.Data.ES.Models;
using Nest;
using SearchInput = FC.Codeflix.Catalog.Domain.Repositories.DTOs.SearchInput;

namespace FC.Codeflix.Catalog.Infra.Data.ES.Repositories;
public class CategoryRepository : ICategoryRepository
{
    private readonly IElasticClient _client;

    public CategoryRepository(IElasticClient client)
    {
        _client = client;
    }

    public Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task SaveAsync(Category entity, CancellationToken cancellationToken)
    {
        var model = CategoryModel.FromEntity(entity);
        await _client
            .IndexDocumentAsync(model, cancellationToken);
    }

    public Task<SearchOutput<Category>> SearchAsync(
        SearchInput input, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
