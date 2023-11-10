using FC.Codeflix.Catalog.Tests.Shared;

namespace FC.Codeflix.Catalog.UnitTests.Domain.Entity.Genre;

public class GenreTestFixture
{
    private readonly GenreDataGenerator _dataGenerator = new();

    public Catalog.Domain.Entity.Genre GetValidGenre()
        => _dataGenerator.GetValidGenre();
}

[CollectionDefinition(nameof(GenreTestFixture))]
public class GenreTestFixtureCollection
    : ICollectionFixture<GenreTestFixture>
{ }