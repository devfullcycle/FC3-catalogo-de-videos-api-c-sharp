using FC.Codeflix.Catalog.Domain.Repositories.DTOs;
using FC.Codeflix.Catalog.Infra.Data.ES.Models;
using FC.Codeflix.Catalog.IntegrationTests.Genre.Common;

namespace FC.Codeflix.Catalog.IntegrationTests.Genre.SearchGenre;

public class SearchGenreTestFixture : GenreTestFixture
{
    public IList<GenreModel> GetGenreModelList(IEnumerable<string> names)
        => DataGenerator.GetGenreModelList(names);

    public IList<GenreModel> CloneGenresListOrdered(List<GenreModel> examples, string orderBy, SearchOrder inputOrder)
        => DataGenerator.CloneGenresListOrdered(examples, orderBy, inputOrder);
}

[CollectionDefinition(nameof(SearchGenreTestFixture))]
public class SearchGenreTestFixtureCollection
    : ICollectionFixture<SearchGenreTestFixture>
{}