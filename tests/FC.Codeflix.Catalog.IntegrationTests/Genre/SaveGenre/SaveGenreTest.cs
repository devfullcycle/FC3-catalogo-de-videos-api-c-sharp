using FC.Codeflix.Catalog.Application.UseCases.Genre.SaveGenre;
using FC.Codeflix.Catalog.Infra.Data.ES.Models;
using FC.Codeflix.Catalog.IntegrationTests.Genre.Common;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FC.Codeflix.Catalog.IntegrationTests.Genre.SaveGenre;

[Collection(nameof(GenreTestFixture))]
public class SaveGenreTest: IDisposable
{
    private readonly GenreTestFixture _fixture;

    public SaveGenreTest(GenreTestFixture fixture)
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
        var input = new SaveGenreInput(Guid.NewGuid());

        var output = await mediator.Send(input);

        var persisted = await elasticClient
            .GetAsync<GenreModel>(input.Id);
        persisted.Found.Should().BeTrue();
        var document = persisted.Source;
        document.Should().NotBeNull();
        document.Id.Should().Be(input.Id);
        output.Should().NotBeNull();
        output.Should().BeEquivalentTo(document);
    }

    public void Dispose() => _fixture.DeleteAll();
}