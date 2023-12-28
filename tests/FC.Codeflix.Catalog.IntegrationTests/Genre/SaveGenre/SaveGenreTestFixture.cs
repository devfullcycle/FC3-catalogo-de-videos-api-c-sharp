using FC.Codeflix.Catalog.Application.UseCases.Genre.SaveGenre;
using FC.Codeflix.Catalog.IntegrationTests.Genre.Common;

namespace FC.Codeflix.Catalog.IntegrationTests.Genre.SaveGenre;

public class SaveGenreTestFixture : GenreTestFixture
{
    public SaveGenreInput GetValidInput()
        => DataGenerator.GetValidSaveGenreInput();

    public SaveGenreInput GetInvalidInput()
        => DataGenerator.GetInvalidSaveGenreInput();
}

[CollectionDefinition(nameof(SaveGenreTestFixture))]
public class SaveGenreTestFixtureCollection
    : ICollectionFixture<SaveGenreTestFixture>
{ }
