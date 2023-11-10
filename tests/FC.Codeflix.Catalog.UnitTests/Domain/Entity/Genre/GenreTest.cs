using FC.Codeflix.Catalog.Domain.Exceptions;
using FluentAssertions;

namespace FC.Codeflix.Catalog.UnitTests.Domain.Entity.Genre;
[Collection(nameof(GenreTestFixture))]
public class GenreTest
{
    private readonly GenreTestFixture _fixture;

    public GenreTest(GenreTestFixture fixture)
        => _fixture = fixture;
    
    [Fact(DisplayName = nameof(Constructor_WhenValidArguments_Instantiate))]
    [Trait("Domain", "Genre - Aggregates")]
    public void Constructor_WhenValidArguments_Instantiate()
    {
        var example = _fixture.GetValidGenre();

        var genre = new Catalog.Domain.Entity.Genre(
            example.Id,
            example.Name,
            example.IsActive,
            example.CreatedAt,
            example.Categories);

        genre.Id.Should().Be(example.Id);
        genre.Name.Should().Be(example.Name);
        genre.IsActive.Should().Be(example.IsActive);
        genre.CreatedAt.Should().Be(example.CreatedAt);
        genre.Categories.Should().BeEquivalentTo(example.Categories);
        genre.CreatedAt.Should().Be(example.CreatedAt);
    }
    
    [Fact(DisplayName = nameof(Constructor_WhenCategoriesIsNull_Instantiate))]
    [Trait("Domain", "Genre - Aggregates")]
    public void Constructor_WhenCategoriesIsNull_Instantiate()
    {
        var example = _fixture.GetValidGenre();

        var genre = new Catalog.Domain.Entity.Genre(
            example.Id,
            example.Name,
            example.IsActive,
            example.CreatedAt,
            null);

        genre.Id.Should().Be(example.Id);
        genre.Name.Should().Be(example.Name);
        genre.IsActive.Should().Be(example.IsActive);
        genre.CreatedAt.Should().Be(example.CreatedAt);
        genre.Categories.Should().BeEmpty();
        genre.CreatedAt.Should().Be(example.CreatedAt);
    }
    
    [Fact(DisplayName = nameof(Constructor_WhenInvalidId_ThrowsException))]
    [Trait("Domain", "Genre - Aggregates")]
    public void Constructor_WhenInvalidId_ThrowsException()
    {
        var example = _fixture.GetValidGenre();

        var action = () 
            => new Catalog.Domain.Entity.Genre(
                Guid.Empty,
                example.Name,
                example.IsActive,
                example.CreatedAt,
                example.Categories);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Id should not be empty or null");
    }
    
    [Fact(DisplayName = nameof(Constructor_WhenInvalidId_ThrowsException))]
    [Trait("Domain", "Genre - Aggregates")]
    public void Constructor_WhenInvalidName_ThrowsException()
    {
        var example = _fixture.GetValidGenre();

        var action = () 
            => new Catalog.Domain.Entity.Genre(
                example.Id,
                null,
                example.IsActive,
                example.CreatedAt,
                example.Categories);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Name should not be empty or null");
    }

}