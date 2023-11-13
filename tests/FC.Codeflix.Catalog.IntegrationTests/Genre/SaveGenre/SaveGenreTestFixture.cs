using FC.Codeflix.Catalog.Application.UseCases.Genre.SaveGenre;
using FC.Codeflix.Catalog.IntegrationTests.Genre.Common;

namespace FC.Codeflix.Catalog.IntegrationTests.Genre.SaveGenre;

public class SaveGenreTestFixture : GenreTestFixture
{
    public SaveGenreInput GetValidInput()
        => new(Guid.Parse("d2b374bc-0ef5-45f6-a8e5-71d715730281"));

    public SaveGenreInput GetInvalidInput()
        => new(Guid.Empty);
}