using FC.Codeflix.Catalog.Application.UseCases.CastMember.SearchCastMember;
using FC.Codeflix.Catalog.Domain.Repositories.DTOs;
using MediatR;

namespace FC.Codeflix.Catalog.Api.CastMembers;

[ExtendObjectType(OperationTypeNames.Query)]
public class CastMemberQueries
{
    public async Task<SearchCastMemberPayload> GetCastMembersAsync(
        [Service] IMediator mediator,
        int page = 1,
        int perPage = 10,
        string search = "",
        string sort = "",
        SearchOrder direction = SearchOrder.Asc,
        CancellationToken cancellationToken = default)
    {
        var input = new SearchCastMemberInput(page, perPage, search, sort, direction);
        var output = await mediator.Send(input, cancellationToken);
        return SearchCastMemberPayload.FromSearchListOutput(output);
    }
}