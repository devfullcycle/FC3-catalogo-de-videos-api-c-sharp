using Bogus.DataSets;
using FC.Codeflix.Catalog.Domain.Exceptions;
using FluentAssertions;
using DomainEntity = FC.Codeflix.Catalog.Domain.Entity;

namespace FC.Codeflix.Catalog.UnitTests.Domain.Entity.Category;

[Collection(nameof(CategoryTestFixture))]
public class CategoryTest
{
    private readonly CategoryTestFixture _categoryTestFixture;

    public CategoryTest(CategoryTestFixture categoryTestFixture)
        => _categoryTestFixture = categoryTestFixture;

    [Fact(DisplayName = nameof(Instantiate))]
    [Trait("Domain", "Category - Aggregates")]
    public void Instantiate()
    {
        var validCategory = _categoryTestFixture.GetValidCategory();

        var category = new DomainEntity.Category(
            validCategory.Id,
            validCategory.Name,
            validCategory.Description,
            validCategory.CreatedAt);

        category.Should().NotBeNull();
        category.Id.Should().Be(validCategory.Id);
        category.Name.Should().Be(validCategory.Name);
        category.Description.Should().Be(validCategory.Description);
        category.CreatedAt.Should().Be(validCategory.CreatedAt);
        category.IsActive.Should().BeTrue();
    }


    [Theory(DisplayName = nameof(InstantiateWithIsActive))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData(true)]
    [InlineData(false)]
    public void InstantiateWithIsActive(bool isActive)
    {
        var validCategory = _categoryTestFixture.GetValidCategory();

        var category = new DomainEntity.Category(
            validCategory.Id,
            validCategory.Name,
            validCategory.Description,
            validCategory.CreatedAt,
            isActive);

        category.Should().NotBeNull();
        category.Id.Should().Be(validCategory.Id);
        category.Name.Should().Be(validCategory.Name);
        category.Description.Should().Be(validCategory.Description);
        category.CreatedAt.Should().Be(validCategory.CreatedAt);
        category.IsActive.Should().Be(isActive);
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenIdIsEmpty))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantiateErrorWhenIdIsEmpty()
    {
        var validCategory = _categoryTestFixture.GetValidCategory();

        Action action =
            () => new DomainEntity.Category(
                Guid.Empty,
                validCategory.Name,
                validCategory.Description,
                validCategory.CreatedAt,
                validCategory.IsActive);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Id should not be empty or null");
    }

    [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsEmpty))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("   ")]
    public void InstantiateErrorWhenNameIsEmpty(string? name)
    {
        var validCategory = _categoryTestFixture.GetValidCategory();

        Action action =
            () => new DomainEntity.Category(
                validCategory.Id,
                name,
                validCategory.Description,
                validCategory.CreatedAt,
                validCategory.IsActive);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Name should not be empty or null");
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsNull))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantiateErrorWhenDescriptionIsNull()
    {
        var validCategory = _categoryTestFixture.GetValidCategory();

        Action action =
            () => new DomainEntity.Category(
                validCategory.Id,
                validCategory.Name,
                null!,
                validCategory.CreatedAt,
                validCategory.IsActive);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Description should not be null");
    }
}

