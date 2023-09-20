using FC.Codeflix.Catalog.Application.Common;
using FC.Codeflix.Catalog.Application.UseCases.Category.Common;
using MediatR;

namespace FC.Codeflix.Catalog.Application.UseCases.Category.SearchCategory;
public interface ISearchCategory
    : IRequestHandler<SearchCategoryInput, SearchListOutput<CategoryModelOutput>>
{
}
