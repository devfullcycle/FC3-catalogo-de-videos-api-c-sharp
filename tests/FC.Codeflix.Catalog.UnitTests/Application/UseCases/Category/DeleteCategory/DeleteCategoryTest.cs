using FC.Codeflix.Catalog.Application.UseCases.Category.DeleteCategory;
using FC.Codeflix.Catalog.UnitTests.Application.UseCases.Category.Common;
using NSubstitute;
using UseCase = FC.Codeflix.Catalog.Application.UseCases.Category.DeleteCategory;

namespace FC.Codeflix.Catalog.UnitTests.Application.UseCases.Category.DeleteCategory;

[Collection(nameof(CategoryUseCaseFixture))]
public class DeleteCategoryTest
{
    private readonly CategoryUseCaseFixture _fixture;

    public DeleteCategoryTest(CategoryUseCaseFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(DeleteCategory))]
    [Trait("Application", "[UseCase] DeleteCategory")]
    public async Task DeleteCategory()
    {
        var repository = _fixture.GetMockRepository();
        var useCase = new UseCase.DeleteCategory(repository);
        var input = new DeleteCategoryInput(Guid.NewGuid());

        await useCase.Handle(input, CancellationToken.None);

        await repository.Received(1).DeleteAsync(
            input.Id, Arg.Any<CancellationToken>());
    }
}
