using FC.Codeflix.Catalog.UnitTests.Application.UseCases.Video.Common;
using DomainEntity = FC.Codeflix.Catalog.Domain.Entity;

namespace FC.Codeflix.Catalog.UnitTests.Application.UseCases.Video.SearchVideo;

public class SearchVideoUseCaseTestFixture : VideoUseCaseTestFixture
{
    public SearchVideoInput GetSearchInput()
    {
        var random = new Random();
        return new SearchVideoInput(
            page: random.Next(1, 10),
            perPage: random.Next(10, 20),
            search: DataGenerator.Faker.Commerce.ProductName(),
            orderBy: DataGenerator.Faker.Commerce.ProductName(),
            order: random.Next(0, 2) == 0
                ? SearchOrder.Asc
                : SearchOrder.Desc);
    }
    
    public List<DomainEntity.Video> GetVideoList(int count = 10)
        => Enumerable
            .Range(0, count)
            .Select(_ => GetValidVideo())
            .ToList();
}

[CollectionDefinition(nameof(SearchVideoUseCaseTestFixture))]
public class SearchVideoTestFixtureCollection
    : ICollectionFixture<SearchVideoUseCaseTestFixture>
{
}