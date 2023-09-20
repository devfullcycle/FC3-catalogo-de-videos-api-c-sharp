using FC.Codeflix.Catalog.Application.UseCases.Category.SearchCategory;
using FC.Codeflix.Catalog.Domain.Repositories.DTOs;
using FC.Codeflix.Catalog.UnitTests.Application.UseCases.Category.Common;
using DomainEntity = FC.Codeflix.Catalog.Domain.Entity;

namespace FC.Codeflix.Catalog.UnitTests.Application.UseCases.Category.SearchCategory;
public class SearchCategoryTestFixture
    : CategoryUseCaseFixture
{
    public SearchCategoryInput GetSearchInput()
    {
        var random = new Random();
        return new SearchCategoryInput(
            page: random.Next(1, 10),
            perPage: random.Next(10, 20),
            search: Faker.Commerce.ProductName(),
            orderBy: Faker.Commerce.ProductName(),
            order: random.Next(0, 2) == 0 
                ? SearchOrder.Asc
                : SearchOrder.Desc);
    }

    public List<DomainEntity.Category> GetCategoryList(int length = 10)
        => Enumerable
            .Range(0, length)
            .Select(_ => GetValidCategory())
            .ToList();
}

[CollectionDefinition(nameof(SearchCategoryTestFixture))]
public class SearchCategoryTestFixtureCollection
    : ICollectionFixture<SearchCategoryTestFixture>
{ }
