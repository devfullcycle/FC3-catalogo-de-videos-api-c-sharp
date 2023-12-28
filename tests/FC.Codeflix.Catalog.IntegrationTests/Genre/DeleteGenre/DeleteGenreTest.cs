using FC.Codeflix.Catalog.Application.UseCases.Genre.DeleteGenre;
using FC.Codeflix.Catalog.Domain.Exceptions;
using FC.Codeflix.Catalog.Infra.Data.ES.Models;
using FC.Codeflix.Catalog.IntegrationTests.Genre.Common;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Nest;

namespace FC.Codeflix.Catalog.IntegrationTests.Genre.DeleteGenre;

[Collection(nameof(GenreTestFixture))]
public class DeleteGenreTest: IDisposable
{
    private readonly GenreTestFixture _fixture;

    public DeleteGenreTest(GenreTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(DeleteGenre_WhenReceivesAnExistingId_DeletesGenre))]
    [Trait("Integration", "[UseCase] DeleteGenre")]
    public async Task DeleteGenre_WhenReceivesAnExistingId_DeletesGenre()
    {
        var serviceProvider = _fixture.ServiceProvider;
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        var elasticClient = _fixture.ElasticClient;
        var genresExample = _fixture.GetGenreModelList();
        await elasticClient.IndexManyAsync(genresExample);
        var input = new DeleteGenreInput(genresExample[3].Id);

        await mediator.Send(input, CancellationToken.None);

        var deletedGenre = await elasticClient.GetAsync<GenreModel>(input.Id);
        deletedGenre.Found.Should().BeFalse();
    }

    [Fact(DisplayName = nameof(DeleteGenre_WhenReceivesANonExistingId_ThrowsException))]
    [Trait("Integration", "[UseCase] DeleteGenre")]
    public async Task DeleteGenre_WhenReceivesANonExistingId_ThrowsException()
    {
        var serviceProvider = _fixture.ServiceProvider;
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        var elasticClient = _fixture.ElasticClient;
        var genresExample = _fixture.GetGenreModelList();
        await elasticClient.IndexManyAsync(genresExample);
        var input = new DeleteGenreInput(Guid.NewGuid());

        var action = async () => await mediator.Send(input, CancellationToken.None);

        await action.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Genre '{input.Id}' not found.");
    }

    public void Dispose() => _fixture.DeleteAll();
}