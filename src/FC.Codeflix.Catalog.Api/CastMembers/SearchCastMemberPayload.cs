using FC.Codeflix.Catalog.Api.Common;
using FC.Codeflix.Catalog.Application.Common;
using FC.Codeflix.Catalog.Application.UseCases.CastMember.Common;

namespace FC.Codeflix.Catalog.Api.CastMembers;

public class SearchCastMemberPayload : SearchPayload<CastMemberPayload>
{
    public static SearchCastMemberPayload FromSearchListOutput(SearchListOutput<CastMemberModelOutput> output)
        => new()
        {
            CurrentPage = output.CurrentPage,
            PerPage = output.PerPage,
            Total = output.Total,
            Items = output.Items.Select(CastMemberPayload.FromCastMemberModelOutput).ToList()
        };
}