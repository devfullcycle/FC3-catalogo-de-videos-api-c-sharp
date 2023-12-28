using FC.Codeflix.Catalog.Application.UseCases.Genre.SaveGenre;
using FC.Codeflix.Catalog.UnitTests.Application.UseCases.Genre.Common;

namespace FC.Codeflix.Catalog.UnitTests.Application.UseCases.Genre.SaveGenre;

public class SaveGenreUseCaseTestFixture : GenreUseCaseTestFixture
{
    public SaveGenreInput GetValidInput()
        => DataGenerator.GetValidSaveGenreInput();

    public SaveGenreInput GetInvalidInput()
        => DataGenerator.GetInvalidSaveGenreInput();
}

[CollectionDefinition(nameof(SaveGenreUseCaseTestFixture))]
public class SaveGenreUseCaseTestFixtureCollection
    : ICollectionFixture<SaveGenreUseCaseTestFixture>
{ }