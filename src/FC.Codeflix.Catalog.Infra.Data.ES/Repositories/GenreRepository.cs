using FC.Codeflix.Catalog.Domain.Entity;
using FC.Codeflix.Catalog.Domain.Exceptions;
using FC.Codeflix.Catalog.Domain.Repositories;
using FC.Codeflix.Catalog.Domain.Repositories.DTOs;
using FC.Codeflix.Catalog.Infra.Data.ES.Models;
using Nest;
using SearchInput = FC.Codeflix.Catalog.Domain.Repositories.DTOs.SearchInput;

namespace FC.Codeflix.Catalog.Infra.Data.ES.Repositories;

public class GenreRepository : IGenreRepository
{
    private readonly IElasticClient _client;

    public GenreRepository(IElasticClient client)
    {
        _client = client;
    }

    public async Task SaveAsync(Genre entity, CancellationToken cancellationToken)
    {
        var model = GenreModel.FromEntity(entity);
        await _client
            .IndexDocumentAsync(model, cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var response = await _client.DeleteAsync<GenreModel>(id, ct: cancellationToken);
        if (response.Result == Result.NotFound)
        {
            throw new NotFoundException($"Genre '{id}' not found.");
        }
    }

    public async Task<SearchOutput<Genre>> SearchAsync(SearchInput input, CancellationToken cancellationToken)
    {
        var response = await _client
            .SearchAsync<GenreModel>(s => s
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

        var genres = response.Documents
            .Select(doc => doc.ToEntity())
            .ToList();

        return new SearchOutput<Genre>(
            input.Page,
            input.PerPage,
            (int)response.Total,
            genres);
    }

    public async Task<IEnumerable<Genre>> GetGenresByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken)
    {
        var response = await _client.SearchAsync<GenreModel>(s => s
            .Query(q => q
                .Bool(b => b
                    .Filter(f => f
                        // .Terms(t => t
                        //     .Field(genre => genre.Id)
                        //     .Terms(ids)
                        // )
                        .Ids(i => i.Values(ids))
                    )
                )
            ), ct: cancellationToken
        );

        return response.Documents
            .Select(doc => doc.ToEntity())
            .ToList();
    }

    private static Func<SortDescriptor<GenreModel>, IPromise<IList<ISort>>> BuildSortExpression(
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