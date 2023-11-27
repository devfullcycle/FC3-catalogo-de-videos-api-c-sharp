using FC.Codeflix.Catalog.Domain.Repositories;
using FC.Codeflix.Catalog.Infra.Data.ES.Models;
using FC.Codeflix.Catalog.Tests.Shared;
using NSubstitute;
using DomainEntity = FC.Codeflix.Catalog.Domain.Entity;

namespace FC.Codeflix.Catalog.UnitTests.Application.UseCases.Genre.Common;

public class GenreUseCaseTestFixture
{
    public GenreDataGenerator DataGenerator { get; } = new();

    public IGenreRepository GetMockRepository()
        => Substitute.For<IGenreRepository>();

    public DomainEntity.Genre GetValidGenre()
        => DataGenerator.GetValidGenre();

    public IList<DomainEntity.Genre> GetGenreList(int count = 10)
        => DataGenerator.GetGenreList(count);
}

[CollectionDefinition(nameof(GenreUseCaseTestFixture))]
public class GenreUseCaseTestFixtureCollection
    : ICollectionFixture<GenreUseCaseTestFixture>
{ }