using FC.Codeflix.Catalog.Application.UseCases.Category.Common;
using MediatR;

namespace FC.Codeflix.Catalog.Application.UseCases.Category.SaveCategory;
public interface ISaveCategory
    : IRequestHandler<SaveCategoryInput, CategoryModelOutput>
{
}
