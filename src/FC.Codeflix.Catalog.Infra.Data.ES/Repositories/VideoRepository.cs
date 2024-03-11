using FC.Codeflix.Catalog.Domain.Entity;
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

    public Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<SearchOutput<Video>> SearchAsync(SearchInput input, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}