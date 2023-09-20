using FC.Codeflix.Catalog.Application.Common;
using FC.Codeflix.Catalog.Application.UseCases.Category.Common;
using FC.Codeflix.Catalog.Domain.Repositories.DTOs;
using MediatR;

namespace FC.Codeflix.Catalog.Application.UseCases.Category.SearchCategory;
public class SearchCategoryInput
    : SearchListInput, IRequest<SearchListOutput<CategoryModelOutput>>
{
    public SearchCategoryInput(
        int page = 1,
        int perPage = 20,
        string search = "",
        string orderBy = "",
        SearchOrder order = SearchOrder.Asc)
        : base(page, perPage, search, orderBy, order)
    {
    }
}
