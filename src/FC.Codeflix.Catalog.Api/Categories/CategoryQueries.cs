using FC.Codeflix.Catalog.Application.UseCases.Category.SearchCategory;
using FC.Codeflix.Catalog.Domain.Repositories.DTOs;
using MediatR;

namespace FC.Codeflix.Catalog.Api.Categories;

[ExtendObjectType(OperationTypeNames.Query)]
public class CategoryQueries
{
    public async Task<SearchCategoryPayload> GetCategoriesAsync(
        [Service] IMediator mediator,
        int page = 1,
        int perPage = 10,
        string search = "",
        string sort = "",
        SearchOrder direction = SearchOrder.Asc,
        CancellationToken cancellationToken = default)
    {
        var input = new SearchCategoryInput(page, perPage, search, sort, direction);
        var output = await mediator.Send(input, cancellationToken);
        return SearchCategoryPayload.FromSearchListOutput(output);
    }
}
