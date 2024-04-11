using FC.Codeflix.Catalog.Application.UseCases.Video.SearchVideo;
using FC.Codeflix.Catalog.Domain.Repositories.DTOs;
using MediatR;

namespace FC.Codeflix.Catalog.Api.Videos;

[ExtendObjectType(OperationTypeNames.Query)]
public class VideoQueries
{
    public async Task<SearchVideoPayload> GetVideosAsync(
        [Service] IMediator mediator,
        int page = 1,
        int perPage = 10,
        string search = "",
        string sort = "",
        SearchOrder direction = SearchOrder.Asc,
        CancellationToken cancellationToken = default)
    {
        var input = new SearchVideoInput(page, perPage, search, sort, direction);
        var output = await mediator.Send(input, cancellationToken);
        return SearchVideoPayload.FromSearchListOutput(output);
    }
}