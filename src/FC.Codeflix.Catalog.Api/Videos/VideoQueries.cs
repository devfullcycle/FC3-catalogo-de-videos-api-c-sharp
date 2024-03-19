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
        throw new NotImplementedException();
    }
}