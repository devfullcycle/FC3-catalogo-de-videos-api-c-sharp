using FC.Codeflix.Catalog.E2ETests.GraphQL.Genres.Common;
using FC.Codeflix.Catalog.Infra.Data.ES.Models;
using RepositoryDTOs = FC.Codeflix.Catalog.Domain.Repositories.DTOs;
namespace FC.Codeflix.Catalog.E2ETests.GraphQL.Genres.SearchGenre;

public class SearchGenreTestFixture : GenreTestFixture
{
    public IList<GenreModel> GetGenreModelList(IEnumerable<string> names)
        => DataGenerator.GetGenreModelList(names);

    public IList<GenreModel> CloneGenresListOrdered(
        List<GenreModel> examples,
        string orderBy,
        RepositoryDTOs.SearchOrder direction)
        => DataGenerator.CloneGenresListOrdered(examples, orderBy, direction);
}

[CollectionDefinition(nameof(SearchGenreTestFixture))]
public class SearchGenreTestFixtureCollection: ICollectionFixture<SearchGenreTestFixture>
{}