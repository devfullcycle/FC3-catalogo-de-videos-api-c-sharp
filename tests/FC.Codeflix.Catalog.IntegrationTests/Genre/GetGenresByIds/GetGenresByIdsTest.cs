using FC.Codeflix.Catalog.Infra.Data.ES;
using FC.Codeflix.Catalog.IntegrationTests.Genre.Common;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using UseCase = FC.Codeflix.Catalog.Application.UseCases.Genre.GetGenresByIds;

namespace FC.Codeflix.Catalog.IntegrationTests.Genre.GetGenresByIds;

[Collection(nameof(GenreTestFixture))]
public class GetGenresByIdsTest : IDisposable
{
    private readonly GenreTestFixture _fixture;

    public GetGenresByIdsTest(GenreTestFixture fixture)
    {
        _fixture = fixture;
    }
    
    [Fact(DisplayName = nameof(GetGenresByIdsTest))]
    [Trait("Integration", "[UseCase] GetGenresByIds")]
    public async Task GetGenresByIds_WhenReceivesAValidInput_ReturnsGenres()
    {
        var elasticClient = _fixture.ElasticClient;
        var genres = _fixture.GetGenreModelList();
        await elasticClient.IndexManyAsync(genres);
        await elasticClient.Indices.RefreshAsync(ElasticsearchIndices.Genre);
        var expectedOutput = new[]
        {
            genres[3], genres[5]
        };
        var ids = expectedOutput.Select(x => x.Id).ToList();
        var input = new UseCase.GetGenresByIdsInput(ids);
        var serviceProvider = _fixture.ServiceProvider;
        var mediator = serviceProvider.GetRequiredService<IMediator>();

        var output = await mediator.Send(input, CancellationToken.None);

        output.Should().BeEquivalentTo(expectedOutput);
    }


    public void Dispose() => _fixture.DeleteAll();
}