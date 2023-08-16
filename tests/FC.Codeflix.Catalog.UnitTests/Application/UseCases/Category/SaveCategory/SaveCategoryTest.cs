using FC.Codeflix.Catalog.Domain.Exceptions;
using FluentAssertions;
using Moq;
using DomainEntity = FC.Codeflix.Catalog.Domain.Entity;
using UseCase = FC.Codeflix.Catalog.Application.UseCases.Category.SaveCategory;

namespace FC.Codeflix.Catalog.UnitTests.Application.UseCases.Category.SaveCategory;
[Collection(nameof(SaveCategoryTestFixture))]
public class SaveCategoryTest
{
    private readonly SaveCategoryTestFixture _fixture;

    public SaveCategoryTest(SaveCategoryTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(SaveValidCategory))]
    [Trait("Application", "[UseCase] SaveCategory")]
    public async Task SaveValidCategory()
    {
        var repository = _fixture.GetMockRepository();
        var useCase = new UseCase.SaveCategory(
            repository.Object);
        var input = _fixture.GetValidInput();

        var output = await useCase.Handle(input, CancellationToken.None);

        repository.Verify(x => x.SaveAsync(
            It.IsAny<DomainEntity.Category>(),
            It.IsAny<CancellationToken>()),
            Times.Once);
        output.Should().NotBeNull();
        output.Id.Should().Be(input.Id);
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.CreatedAt.Should().Be(input.CreatedAt);
        output.IsActive.Should().Be(input.IsActive);
    }

    [Fact(DisplayName = nameof(SaveInvalidCategory))]
    [Trait("Application", "[UseCase] SaveCategory")]
    public async Task SaveInvalidCategory()
    {
        var repository = _fixture.GetMockRepository();
        var useCase = new UseCase.SaveCategory(
            repository.Object);
        var input = _fixture.GetInvalidInput();

        var action = async () => await useCase.Handle(input, CancellationToken.None);

        repository.Verify(x => x.SaveAsync(
            It.IsAny<DomainEntity.Category>(),
            It.IsAny<CancellationToken>()),
            Times.Never);
        await action
            .Should()
            .ThrowAsync<EntityValidationException>()
            .WithMessage("Name should not be empty or null");
    }
}
