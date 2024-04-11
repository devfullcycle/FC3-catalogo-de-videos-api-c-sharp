using FC.Codeflix.Catalog.Domain.Entity;
using FC.Codeflix.Catalog.Domain.Exceptions;
using FC.Codeflix.Catalog.Domain.Repositories;
using FC.Codeflix.Catalog.Domain.Repositories.DTOs;
using FC.Codeflix.Catalog.Infra.Data.ES.Models;
using Nest;
using SearchInput = FC.Codeflix.Catalog.Domain.Repositories.DTOs.SearchInput;

namespace FC.Codeflix.Catalog.Infra.Data.ES.Repositories;

public class VideoRepository : IVideoRepository
{
    private readonly IElasticClient _elasticClient;

    public VideoRepository(IElasticClient elasticClient)
    {
        _elasticClient = elasticClient;
    }

    public async Task SaveAsync(Video entity, CancellationToken cancellationToken)
    {
        var model = VideoModel.FromEntity(entity);
        await _elasticClient.IndexDocumentAsync(model, cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var response = await _elasticClient.DeleteAsync<VideoModel>(id, ct: cancellationToken);
        if (response.Result == Result.NotFound)
        {
            throw new NotFoundException($"Video '{id}' not found.");
        }
    }

    public async Task<SearchOutput<Video>> SearchAsync(SearchInput input, CancellationToken cancellationToken)
    {
        var response = await _elasticClient
            .SearchAsync<VideoModel>(s => s
                    .Query(q => q
                        .Match(m => m
                            .Field(f => f.Title)
                            .Query(input.Search)
                        )
                    )
                    .From(input.From)
                    .Size(input.PerPage)
                    .Sort(BuildSortExpression(input.OrderBy, input.Order)),
                ct: cancellationToken);
        
        var videos = response.Documents
            .Select(doc => doc.ToEntity())
            .ToList();
        
        return new SearchOutput<Video>(
            input.Page,
            input.PerPage,
            (int)response.Total,
            videos);
    }
    
    private static Func<SortDescriptor<VideoModel>, IPromise<IList<ISort>>> BuildSortExpression(
        string orderBy, SearchOrder order)
        => (orderBy.ToLower(), order) switch
        {
            ("title", SearchOrder.Asc) => sort => sort
                .Ascending(f => f.Title.Suffix("keyword"))
                .Ascending(f => f.Id),
            ("title", SearchOrder.Desc) => sort => sort
                .Descending(f => f.Title.Suffix("keyword"))
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
                .Ascending(f => f.Title.Suffix("keyword"))
                .Ascending(f => f.Id)
        };
}