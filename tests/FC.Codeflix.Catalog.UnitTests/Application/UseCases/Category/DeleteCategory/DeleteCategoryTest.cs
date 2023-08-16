using FC.Codeflix.Catalog.UnitTests.Application.UseCases.Category.Common;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        var useCase = new UseCase.DeleteCategory(
            repository.Object);
        var input = new DeleteCategoryInput(Guid.NewGuid());

        await useCase.Handle(input, CancellationToken.None);

        repository.Verify(x => x.DeleteAsync(
            input.Id, It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
