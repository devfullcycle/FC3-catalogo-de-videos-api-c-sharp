using FC.Codeflix.Catalog.Domain.Repositories.DTOs;
using FC.Codeflix.Catalog.UnitTests.Application.UseCases.Genre.Common;
using DomainEntity = FC.Codeflix.Catalog.Domain.Entity;
namespace FC.Codeflix.Catalog.UnitTests.Application.UseCases.Genre.SearchGenre;

public class SearchGenreUseCaseTestFixture : GenreUseCaseTestFixture
{
    public SearchGenreInput GetSearchInput()
    {
        var random = new Random();
        return new SearchGenreInput(
            page: random.Next(1, 10),
            perPage: random.Next(10, 20),
            search: DataGenerator.Faker.Commerce.ProductName(),
            orderBy: DataGenerator.Faker.Commerce.ProductName(),
            order: random.Next(0, 2) == 0 
                ? SearchOrder.Asc
                : SearchOrder.Desc);
    }

    public List<DomainEntity.Genre> GetGenreList(int length = 10)
        => Enumerable
            .Range(0, length)
            .Select(_ => GetValidGenre())
            .ToList();
}