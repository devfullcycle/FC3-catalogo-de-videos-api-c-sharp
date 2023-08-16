using FC.Codeflix.Catalog.Domain.Repositories;

namespace FC.Codeflix.Catalog.Application.UseCases.Category.DeleteCategory;
public class DeleteCategory
    : IDeleteCategory
{
    private readonly ICategoryRepository _repository;

    public DeleteCategory(ICategoryRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(DeleteCategoryInput request, CancellationToken cancellationToken)
    {
        await _repository.DeleteAsync(request.Id, cancellationToken);
    }
}
