using FC.Codeflix.Catalog.Application.UseCases.Video.SaveVideo;
using FC.Codeflix.Catalog.Infra.Data.ES.Models;
using FC.Codeflix.Catalog.IntegrationTests.Video.Common;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FC.Codeflix.Catalog.IntegrationTests.Video.SaveVideo;

[Collection(nameof(VideoTestFixture))]
public class SaveVideoTest : IDisposable
{
    private readonly VideoTestFixture _fixture;

    public SaveVideoTest(VideoTestFixture fixture)
    {
        _fixture = fixture;
    }
    
    [Fact(DisplayName = nameof(SaveVideo_WhenInputIsValid_PersistsVideo))]
    [Trait("Integration", "[UseCase] SaveVideo")]
    public async Task SaveVideo_WhenInputIsValid_PersistsVideo()
    {
        var serviceProvider = _fixture.ServiceProvider;
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        var elasticClient = _fixture.ElasticClient;
        var input = new SaveVideoInput(Guid.NewGuid());

        var output = await mediator.Send(input);

        var persisted = await elasticClient
            .GetAsync<VideoModel>(input.Id);
        persisted.Found.Should().BeTrue();
        var document = persisted.Source;
        document.Should().NotBeNull();
        document.Id.Should().Be(input.Id);
        output.Should().NotBeNull();
        output.Should().BeEquivalentTo(document);
    }

    public void Dispose() => _fixture.DeleteAll();
}