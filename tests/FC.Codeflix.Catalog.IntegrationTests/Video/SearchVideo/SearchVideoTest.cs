using FC.Codeflix.Catalog.Application.UseCases.Video.SearchVideo;
using FC.Codeflix.Catalog.Domain.Repositories.DTOs;
using FC.Codeflix.Catalog.Infra.Data.ES;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Nest;

namespace FC.Codeflix.Catalog.IntegrationTests.Video.SearchVideo;

[Collection(nameof(SearchVideoTestFixture))]
public class SearchVideoTest
{
    private readonly SearchVideoTestFixture _fixture;

    public SearchVideoTest(SearchVideoTestFixture fixture)
        => _fixture = fixture;
    
    [Theory(DisplayName = nameof(SearchVideo_WhenReceivesValidSearchInput_ReturnFilteredList))]
    [Trait("Integration", "[UseCase] SearchVideo")]
    [InlineData("007", 2, 2, 2, 4)]
    [InlineData("Star", 1, 2, 2, 2)]
    [InlineData("Casino Royal", 1, 2, 1, 1)]
    [InlineData("Terminator", 1, 5, 0, 0)]
    public async Task SearchVideo_WhenReceivesValidSearchInput_ReturnFilteredList(
        string search,
        int page,
        int perPage,
        int expectedItemsCount,
        int expectedTotalCount)
    {
        var serviceProvider = _fixture.ServiceProvider;
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        var elasticClient = _fixture.ElasticClient;
        var videoNamesList = new List<string>() {
            "007: Dr. No",
            "007: Casino Royale",
            "007: GoldFinger",
            "007: Skyfall",
            "Star Wars: Return of the Jedi",
            "Star Wars: The Empire Strikes Back",
            "Interstellar"
        };
        var examples = _fixture.GetVideoModelList(videoNamesList);
        await elasticClient.IndexManyAsync(examples);
        await elasticClient.Indices.RefreshAsync(ElasticsearchIndices.Video);
        var input = new SearchVideoInput(page: page, perPage: perPage, search: search);

        var output = await mediator.Send(input);

        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.CurrentPage.Should().Be(page);
        output.PerPage.Should().Be(perPage);
        output.Total.Should().Be(expectedTotalCount);
        output.Items.Should().HaveCount(expectedItemsCount);

        foreach (var outputItem in output.Items)
        {
            var expected = examples.First(x => x.Id == outputItem.Id);
            outputItem.Title.Should().Be(expected.Title);
            outputItem.Description.Should().Be(expected.Description);
            outputItem.YearLaunched.Should().Be(expected.YearLaunched);
            outputItem.Duration.Should().Be(expected.Duration);
            outputItem.Rating.Should().Be(expected.Rating);
            outputItem.CreatedAt.Should().Be(expected.CreatedAt);
            outputItem.Categories.Should().BeEquivalentTo(expected.Categories);
        }
    }
    
    [Theory(DisplayName = nameof(SearchVideo_WhenReceivesValidSearchInput_ReturnOrderedList))]
    [Trait("Integration", "[UseCase] SearchVideo")]
    [InlineData("title", "asc")]
    [InlineData("title", "desc")]
    [InlineData("id", "asc")]
    [InlineData("id", "desc")]
    [InlineData("createdat", "asc")]
    [InlineData("createdat", "desc")]
    [InlineData("", "desc")]
    public async Task SearchVideo_WhenReceivesValidSearchInput_ReturnOrderedList(
        string orderBy,
        string direction)
    {
        var serviceProvider = _fixture.ServiceProvider;
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        var elasticClient = _fixture.ElasticClient;
        var examples = _fixture.GetVideoModelList();
        await elasticClient.IndexManyAsync(examples);
        await elasticClient.Indices.RefreshAsync(ElasticsearchIndices.Video);
        var input = new SearchVideoInput(
            page: 1,
            perPage: examples.Count,
            orderBy: orderBy,
            order: direction == "asc" ? SearchOrder.Asc : SearchOrder.Desc);
        var expectedList = _fixture.CloneVideosListOrdered(
            examples, orderBy, input.Order);

        var output = await mediator.Send(input);

        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.CurrentPage.Should().Be(input.Page);
        output.PerPage.Should().Be(input.PerPage);
        output.Total.Should().Be(examples.Count);
        output.Items.Should().HaveCount(examples.Count);
        for (int i = 0; i < output.Items.Count; i++)
        {
            var outputItem = output.Items[i];
            var expected = expectedList[i];
            outputItem.Id.Should().Be(expected.Id);
            outputItem.Title.Should().Be(expected.Title);
            outputItem.CreatedAt.Should().Be(expected.CreatedAt);
            outputItem.Categories.Should().BeEquivalentTo(expected.Categories);
        }
    }

    public void Dispose() => _fixture.DeleteAll();
}