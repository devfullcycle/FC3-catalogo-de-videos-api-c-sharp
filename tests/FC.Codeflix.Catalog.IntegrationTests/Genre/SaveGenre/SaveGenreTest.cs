using FC.Codeflix.Catalog.Domain.Exceptions;
using FC.Codeflix.Catalog.Infra.Data.ES.Models;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FC.Codeflix.Catalog.IntegrationTests.Genre.SaveGenre;

[Collection(nameof(SaveGenreTestFixture))]
public class SaveGenreTest: IDisposable
{
    private readonly SaveGenreTestFixture _fixture;

    public SaveGenreTest(SaveGenreTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(SaveGenre_WhenInputIsValid_PersitsGenre))]
    [Trait("Integration", "[UseCase] SaveGenre")]
    public async Task SaveGenre_WhenInputIsValid_PersitsGenre()
    {
        var serviceProvider = _fixture.ServiceProvider;
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        var elasticClient = _fixture.ElasticClient;
        var input = _fixture.GetValidInput();

        var output = await mediator.Send(input);

        var persisted = await elasticClient
            .GetAsync<GenreModel>(input.Id);
        persisted.Found.Should().BeTrue();
        var document = persisted.Source;
        document.Should().NotBeNull();
        document.Id.Should().Be(input.Id);
        document.Name.Should().Be(input.Name);
        document.IsActive.Should().Be(input.IsActive);
        document.CreatedAt.Should().Be(input.CreatedAt);
        document.Categories.Should().BeEquivalentTo(input.Categories);
        output.Should().NotBeNull();
        output.Should().BeEquivalentTo(document);
    }

    [Fact(DisplayName = nameof(SaveGenre_WhenInputIsInvalid_ThrowsException))]
    [Trait("Integration", "[UseCase] SaveGenre")]
    public async Task SaveGenre_WhenInputIsInvalid_ThrowsException()
    {
        var serviceProvider = _fixture.ServiceProvider;
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        var elasticClient = _fixture.ElasticClient;
        var input = _fixture.GetInvalidInput();
        var expectedMessage = "Name should not be empty or null";

        var action = async () => await mediator.Send(input);

        await action.Should().ThrowAsync<EntityValidationException>()
            .WithMessage(expectedMessage);
        var persisted = await elasticClient
            .GetAsync<GenreModel>(input.Id);
        persisted.Found.Should().BeFalse();
    }

    public void Dispose() => _fixture.DeleteAll();
}