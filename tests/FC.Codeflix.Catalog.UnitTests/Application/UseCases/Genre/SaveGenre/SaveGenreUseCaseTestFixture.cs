using FC.Codeflix.Catalog.Application.UseCases.Genre.SaveGenre;
using FC.Codeflix.Catalog.UnitTests.Application.UseCases.Genre.Common;

namespace FC.Codeflix.Catalog.UnitTests.Application.UseCases.Genre.SaveGenre;

public class SaveGenreUseCaseTestFixture : GenreUseCaseTestFixture
{
    public SaveGenreInput GetValidInput()
    {
        var genre = DataGenerator.GetValidGenre();
        return new(
            genre.Id,
            genre.Name,
            genre.IsActive,
            genre.CreatedAt,
            genre.Categories.Select(item => new SaveGenreInputCategory(item.Id, item.Name)));
    }

    public SaveGenreInput GetInvalidInput()
    {
        var genre = DataGenerator.GetValidGenre();
        return new(
            genre.Id,
            null!,
            genre.IsActive,
            genre.CreatedAt,
            genre.Categories.Select(item => new SaveGenreInputCategory(item.Id, item.Name)));
    }
}

[CollectionDefinition(nameof(SaveGenreUseCaseTestFixture))]
public class SaveGenreUseCaseTestFixtureCollection
    : ICollectionFixture<SaveGenreUseCaseTestFixture>
{ }