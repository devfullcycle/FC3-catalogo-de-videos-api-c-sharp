using FC.Codeflix.Catalog.Domain.Repositories;
using FC.Codeflix.Catalog.Tests.Shared;
using NSubstitute;
using DomainEntity = FC.Codeflix.Catalog.Domain.Entity;

namespace FC.Codeflix.Catalog.UnitTests.Application.UseCases.Category.Common;
public class CategoryUseCaseFixture
{
    public CategoryDataGenerator DataGenerator { get; } = new();

    public ICategoryRepository GetMockRepository()
        => Substitute.For<ICategoryRepository>();

    public DomainEntity.Category GetValidCategory()
        => DataGenerator.GetValidCategory();
}

[CollectionDefinition(nameof(CategoryUseCaseFixture))]
public class CategoryUseCaseFixtureCollection
    : ICollectionFixture<CategoryUseCaseFixture>
{ }
