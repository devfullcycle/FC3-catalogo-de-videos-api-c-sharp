using FC.Codeflix.Catalog.Application.Common;
using FC.Codeflix.Catalog.Application.UseCases.Category.Common;
using FC.Codeflix.Catalog.Domain.Repositories;

namespace FC.Codeflix.Catalog.Application.UseCases.Category.SearchCategory;
public class SearchCategory : ISearchCategory
{
    private readonly ICategoryRepository _repository;

    public SearchCategory(ICategoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<SearchListOutput<CategoryModelOutput>> Handle(
        SearchCategoryInput request,
        CancellationToken cancellationToken)
    {
        var searchInput = request.ToSearchInput();
        var categories = await _repository.SearchAsync(searchInput, cancellationToken);
        return new SearchListOutput<CategoryModelOutput>(
            categories.CurrentPage,
            categories.PerPage,
            categories.Total,
            categories.Items.Select(CategoryModelOutput.FromCategory).ToList());
    }
}
