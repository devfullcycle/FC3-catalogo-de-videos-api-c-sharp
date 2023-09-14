using FC.Codeflix.Catalog.Domain.Entity;
using FC.Codeflix.Catalog.Domain.Exceptions;
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

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var response = await _client.DeleteAsync<CategoryModel>(id, ct: cancellationToken);
        if (response.Result == Result.NotFound)
        {
            throw new NotFoundException($"Category '{id}' not found.");
        }
    }

    public async Task SaveAsync(Category entity, CancellationToken cancellationToken)
    {
        var model = CategoryModel.FromEntity(entity);
        await _client
            .IndexDocumentAsync(model, cancellationToken);
    }

    /*
     * {
     *    "query": {
     *       "match": {
     *          "name": {
     *             "query": "action"
     *          }
     *       }
     *    },
     *    "from": 20, 
     *    "size": 10,
     *    "sort": [
     *       { "name.keyword": "asc" },
     *       { "id": "asc" }
     *    ]
     * }
     */
    public async Task<SearchOutput<Category>> SearchAsync(
        SearchInput input, CancellationToken cancellationToken)
    {
        var response = await _client
            .SearchAsync<CategoryModel>(s => s
                .Query(q => q
                    .Match(m => m
                        .Field(f => f.Name)
                        .Query(input.Search)
                    )
                )
                .From(input.From)
                .Size(input.PerPage)
                .Sort(BuildSortExpression(input.OrderBy, input.Order)),
            ct: cancellationToken);

        var categories = response.Documents
            .Select(doc => doc.ToEntity())
            .ToList();

        return new SearchOutput<Category>(
            input.Page,
            input.PerPage,
            (int)response.Total,
            categories);
    }

    private static Func<SortDescriptor<CategoryModel>, IPromise<IList<ISort>>> BuildSortExpression(
        string orderBy, SearchOrder order)
        => (orderBy.ToLower(), order) switch
        {
            ("name", SearchOrder.Asc) => sort => sort
                            .Ascending(f => f.Name.Suffix("keyword"))
                            .Ascending(f => f.Id),
            ("name", SearchOrder.Desc) => sort => sort
                            .Descending(f => f.Name.Suffix("keyword"))
                            .Descending(f => f.Id),
            ("id", SearchOrder.Asc) => sort => sort
                            .Ascending(f => f.Id),
            ("id", SearchOrder.Desc) => sort => sort
                            .Descending(f => f.Id),
            ("createdat", SearchOrder.Asc) => sort => sort
                            .Ascending(f => f.CreatedAt),
            ("createdat", SearchOrder.Desc) => sort => sort
                            .Descending(f => f.CreatedAt),
            _ => sort => sort
                            .Ascending(f => f.Name.Suffix("keyword"))
                            .Ascending(f => f.Id)
        };
}
