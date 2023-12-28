using FC.Codeflix.Catalog.Application.UseCases.Genre.SearchGenre;
using FC.Codeflix.Catalog.Domain.Repositories.DTOs;
using FC.Codeflix.Catalog.Infra.Data.ES;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Nest;

namespace FC.Codeflix.Catalog.IntegrationTests.Genre.SearchGenre;

[Collection(nameof(SearchGenreTestFixture))]
public class SearchGenreTest : IDisposable
{
    private readonly SearchGenreTestFixture _fixture;

    public SearchGenreTest(SearchGenreTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Theory(DisplayName = nameof(SearchGenre_WhenReceivesValidSearchInput_ReturnFilteredList))]
    [Trait("Integration", "[UseCase] SearchGenre")]
    [InlineData("Action", 1, 5, 1, 1)]
    [InlineData("Horror", 1, 5, 3, 3)]
    [InlineData("Horror", 2, 5, 0, 3)]
    [InlineData("Sci-fi", 1, 5, 4, 4)]
    [InlineData("Sci-fi", 1, 2, 2, 4)]
    [InlineData("Sci-fi", 2, 3, 1, 4)]
    [InlineData("Others", 1, 5, 0, 0)]
    [InlineData("Robots", 1, 5, 2, 2)]
    public async Task SearchGenre_WhenReceivesValidSearchInput_ReturnFilteredList(
        string search,
        int page,
        int perPage,
        int expectedItemsCount,
        int expectedTotalCount)
    {
        var serviceProvider = _fixture.ServiceProvider;
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        var elasticClient = _fixture.ElasticClient;
        var genreNamesList = new List<string>() {
            "Action",
            "Horror",
            "Horror - Robots",
            "Horror - Based on Real Facts",
            "Drama",
            "Sci-fi IA",
            "Sci-fi Space",
            "Sci-fi Robots",
            "Sci-fi Future"
        };
        var examples = _fixture.GetGenreModelList(genreNamesList);
        await elasticClient.IndexManyAsync(examples);
        await elasticClient.Indices.RefreshAsync(ElasticsearchIndices.Genre);
        var input = new SearchGenreInput(page: page, perPage: perPage, search: search);

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
            outputItem.Name.Should().Be(expected!.Name);
            outputItem.IsActive.Should().Be(expected.IsActive);
            outputItem.CreatedAt.Should().Be(expected.CreatedAt);
            outputItem.Categories.Should().BeEquivalentTo(expected.Categories);
        }
    }

    [Theory(DisplayName = nameof(SearchGenre_WhenReceivesValidSearchInput_ReturnOrderedList))]
    [Trait("Integration", "[UseCase] SearchGenre")]
    [InlineData("name", "asc")]
    [InlineData("name", "desc")]
    [InlineData("id", "asc")]
    [InlineData("id", "desc")]
    [InlineData("createdat", "asc")]
    [InlineData("createdat", "desc")]
    [InlineData("", "desc")]
    public async Task SearchGenre_WhenReceivesValidSearchInput_ReturnOrderedList(
        string orderBy,
        string direction)
    {
        var serviceProvider = _fixture.ServiceProvider;
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        var elasticClient = _fixture.ElasticClient;
        var examples = _fixture.GetGenreModelList();
        await elasticClient.IndexManyAsync(examples);
        await elasticClient.Indices.RefreshAsync(ElasticsearchIndices.Genre);
        var input = new SearchGenreInput(
            page: 1,
            perPage: examples.Count,
            orderBy: orderBy,
            order: direction == "asc" ? SearchOrder.Asc : SearchOrder.Desc);
        var expectedList = _fixture.CloneGenresListOrdered(
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
            outputItem.Name.Should().Be(expected.Name);
            outputItem.IsActive.Should().Be(expected.IsActive);
            outputItem.CreatedAt.Should().Be(expected.CreatedAt);
            outputItem.Categories.Should().BeEquivalentTo(expected.Categories);
        }
    }

    public void Dispose() => _fixture.DeleteAll();
}