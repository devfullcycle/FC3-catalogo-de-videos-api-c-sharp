using FC.Codeflix.Catalog.Application.Common;
using FC.Codeflix.Catalog.Application.UseCases.CastMember.Common;
using MediatR;

namespace FC.Codeflix.Catalog.Application.UseCases.CastMember.SearchCastMember;

public interface ISearchCastMember
    : IRequestHandler<SearchCastMemberInput, SearchListOutput<CastMemberModelOutput>>
{
    
}