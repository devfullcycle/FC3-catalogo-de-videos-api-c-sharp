using FC.Codeflix.Catalog.Domain.Exceptions;
using FC.Codeflix.Catalog.UnitTests.Application.UseCases.Genre.Common;
using FluentAssertions;
using NSubstitute;
using UseCase = FC.Codeflix.Catalog.Application.UseCases.Genre.SaveGenre;
using DomainEntity = FC.Codeflix.Catalog.Domain.Entity;

namespace FC.Codeflix.Catalog.UnitTests.Application.UseCases.Genre.SaveGenre;

[Collection(nameof(GenreUseCaseTestFixture))]
public class SaveGenreUseCaseTest
{
    private readonly GenreUseCaseTestFixture _fixture;

    public SaveGenreUseCaseTest(GenreUseCaseTestFixture fixture)
    {
        _fixture = fixture;
    }
    
    [Fact(DisplayName = nameof(SaveValidGenre))]
    [Trait("Application", "[UseCase] SaveGenre")]
    public async Task SaveValidGenre()
    {
        var repository = _fixture.GetMockRepository();
        var gateway = _fixture.GetMockAdminCatalogGateway();
        var genre = _fixture.GetValidGenre();
        gateway.GetGenreAsync(
                genre.Id,
                Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(genre));
        var useCase = new UseCase.SaveGenre(repository, gateway);
        var input = new UseCase.SaveGenreInput(genre.Id);
        
        var output = await useCase.Handle(input, CancellationToken.None);
        
        await repository.Received(1).SaveAsync(
            Arg.Any<DomainEntity.Genre>(),
            Arg.Any<CancellationToken>());
        output.Should().NotBeNull();
        output.Id.Should().Be(input.Id);
        output.Name.Should().Be(genre.Name);
        output.CreatedAt.Should().Be(genre.CreatedAt);
        output.IsActive.Should().Be(genre.IsActive);
        output.Categories.Should().BeEquivalentTo(genre.Categories.Select(c => new { c.Id, c.Name }));
    }
}