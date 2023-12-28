using FC.Codeflix.Catalog.Application.UseCases.Genre.GetGenresByIds;
using FC.Codeflix.Catalog.Application.UseCases.Genre.SearchGenre;
using FC.Codeflix.Catalog.Domain.Repositories.DTOs;
using HotChocolate.Resolvers;
using MediatR;

namespace FC.Codeflix.Catalog.Api.Genres;

[ExtendObjectType(OperationTypeNames.Query)]
public class GenreQueries
{
    public async Task<SearchGenrePayload> GetGenresAsync(
        [Service] IMediator mediator,
        int page = 1,
        int perPage = 10,
        string search = "",
        string sort = "",
        SearchOrder direction = SearchOrder.Asc,
        CancellationToken cancellationToken = default)
    {
        var input = new SearchGenreInput(page, perPage, search, sort, direction);
        var output = await mediator.Send(input, cancellationToken);
        return SearchGenrePayload.FromSearchListOutput(output);
    }

    public async Task<GenrePayload?> GetGenreAsync(
        Guid id,
        IResolverContext context,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        return await context.BatchDataLoader<Guid, GenrePayload>(
            async (keys, ct) =>
            {
                var result = await mediator.Send(new GetGenresByIdsInput(keys), ct);
                return result.ToDictionary(x => x.Id, GenrePayload.FromGenreModelOutput);
            }).LoadAsync(id, cancellationToken);
    }
}