using FC.Codeflix.Catalog.Application.Common;
using FC.Codeflix.Catalog.Application.UseCases.CastMember.Common;
using FC.Codeflix.Catalog.Domain.Repositories;

namespace FC.Codeflix.Catalog.Application.UseCases.CastMember.SearchCastMember;

public class SearchCastMember : ISearchCastMember
{
    private readonly ICastMemberRepository _repository;

    public SearchCastMember(ICastMemberRepository repository)
    {
        _repository = repository;
    }

    public async Task<SearchListOutput<CastMemberModelOutput>> Handle(
        SearchCastMemberInput request, CancellationToken cancellationToken)
    {
        var searchInput = request.ToSearchInput();
        var categories = await _repository.SearchAsync(searchInput, cancellationToken);
        return new SearchListOutput<CastMemberModelOutput>(
            categories.CurrentPage,
            categories.PerPage,
            categories.Total,
            categories.Items.Select(CastMemberModelOutput.FromCastMember).ToList());
    }
}