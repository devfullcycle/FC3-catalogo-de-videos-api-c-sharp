using FC.Codeflix.Catalog.Domain.Exceptions;
using FluentAssertions;
using DomainEntity = FC.Codeflix.Catalog.Domain.Entity;

namespace FC.Codeflix.Catalog.UnitTests.Domain.Entity.CastMember;

[Collection(nameof(CastMemberTestFixture))]
public class CastMemberTest
{
    private readonly CastMemberTestFixture _fixture;

    public CastMemberTest(CastMemberTestFixture fixture)
        => _fixture = fixture;

    [Fact(DisplayName = nameof(Constructor_WhenValidArguments_Instantiate))]
    [Trait("Domain", "CastMember - Aggregates")]
    public void Constructor_WhenValidArguments_Instantiate()
    {
        var example = _fixture.GetValidCastMember();

        var castMember = new DomainEntity.CastMember(
            example.Id,
            example.Name,
            example.Type,
            example.CreatedAt);
        
        castMember.Id.Should().Be(example.Id);
        castMember.Name.Should().Be(example.Name);
        castMember.Type.Should().Be(example.Type);
        castMember.CreatedAt.Should().Be(example.CreatedAt);
    }
    
    [Fact(DisplayName = nameof(Constructor_WhenIdIsEmpty_ThrowsException))]
    [Trait("Domain", "CastMember - Aggregates")]
    public void Constructor_WhenIdIsEmpty_ThrowsException()
    {
        var example = _fixture.GetValidCastMember();

        var action = () => new DomainEntity.CastMember(
            Guid.Empty,
            example.Name,
            example.Type,
            example.CreatedAt);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Id should not be empty or null");
    }
    
    [Theory(DisplayName = nameof(Constructor_WhenNameIsNullOrEmpty_ThrowsException))]
    [Trait("Domain", "CastMember - Aggregates")]
    [InlineData(null)]
    [InlineData("")]
    public void Constructor_WhenNameIsNullOrEmpty_ThrowsException(string name)
    {
        var example = _fixture.GetValidCastMember();

        var action = () => new DomainEntity.CastMember(
            example.Id,
            name,
            example.Type,
            example.CreatedAt);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Name should not be empty or null");
    }
    
    [Fact(DisplayName = nameof(Constructor_WhenTypeIsNotDefined_ThrowsException))]
    [Trait("Domain", "CastMember - Aggregates")]
    public void Constructor_WhenTypeIsNotDefined_ThrowsException()
    {
        var example = _fixture.GetValidCastMember();

        var action = () => new DomainEntity.CastMember(
            example.Id,
            example.Name,
            0,
            example.CreatedAt);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Type is not a valid CastMemberType");
    }
}