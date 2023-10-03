using FC.Codeflix.Catalog.Tests.Shared;
using DomainEntity = FC.Codeflix.Catalog.Domain.Entity;

namespace FC.Codeflix.Catalog.UnitTests.Domain.Entity.Category;
public class CategoryTestFixture
{
    public CategoryDataGenerator DataGenerator { get; }
    public CategoryTestFixture()
        => DataGenerator = new CategoryDataGenerator();

    public DomainEntity.Category GetValidCategory()
        => DataGenerator.GetValidCategory();
}

[CollectionDefinition(nameof(CategoryTestFixture))]
public class CategoryTestFixtureCollection
    : ICollectionFixture<CategoryTestFixture>
{ }