using FC.Codeflix.Catalog.Application.Common;
using FC.Codeflix.Catalog.Application.UseCases.CastMember.Common;
using FC.Codeflix.Catalog.Domain.Repositories.DTOs;
using MediatR;

namespace FC.Codeflix.Catalog.Application.UseCases.CastMember.SearchCastMember;

public class SearchCastMemberInput
    : SearchListInput, IRequest<SearchListOutput<CastMemberModelOutput>>
{
    public SearchCastMemberInput(
        int page = 1,
        int perPage = 20,
        string search = "",
        string orderBy = "",
        SearchOrder order = SearchOrder.Asc)
        : base(page, perPage, search, orderBy, order)
    {
    }
}