using FC.Codeflix.Catalog.Application.UseCases.CastMember.SearchCastMember;
using FC.Codeflix.Catalog.Domain.Repositories.DTOs;
using FC.Codeflix.Catalog.Infra.Data.ES;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Nest;

namespace FC.Codeflix.Catalog.IntegrationTests.CastMember.SearchCastMember;

[Collection(nameof(SearchCastMemberTestFixture))]
public class SearchCastMemberTest : IDisposable
{
    private readonly SearchCastMemberTestFixture _fixture;

    public SearchCastMemberTest(SearchCastMemberTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Theory(DisplayName = nameof(SearchCastMember_WhenReceivesValidSearchInput_ReturnFilteredList))]
    [Trait("Integration", "[UseCase] SearchCastMember")]
    [InlineData("Action", 1, 5, 1, 1)]
    [InlineData("Horror", 1, 5, 3, 3)]
    [InlineData("Horror", 2, 5, 0, 3)]
    [InlineData("Sci-fi", 1, 5, 4, 4)]
    [InlineData("Sci-fi", 1, 2, 2, 4)]
    [InlineData("Sci-fi", 2, 3, 1, 4)]
    [InlineData("Others", 1, 5, 0, 0)]
    [InlineData("Robots", 1, 5, 2, 2)]
    public async Task SearchCastMember_WhenReceivesValidSearchInput_ReturnFilteredList(
        string search,
        int page,
        int perPage,
        int expectedItemsCount,
        int expectedTotalCount)
    {
        var serviceProvider = _fixture.ServiceProvider;
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        var elasticClient = _fixture.ElasticClient;
        var castMemberNamesList = new List<string>() {
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
        var examples = _fixture.GetCastMemberModelList(castMemberNamesList);
        await elasticClient.IndexManyAsync(examples);
        await elasticClient.Indices.RefreshAsync(ElasticsearchIndices.CastMember);
        var input = new SearchCastMemberInput(page: page, perPage: perPage, search: search);

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
            outputItem.Type.Should().Be(expected.Type);
            outputItem.CreatedAt.Should().Be(expected.CreatedAt);
        }
    }

    [Theory(DisplayName = nameof(SearchCastMember_WhenReceivesValidSearchInput_ReturnOrderedList))]
    [Trait("Integration", "[UseCase] SearchCastMember")]
    [InlineData("name", "asc")]
    [InlineData("name", "desc")]
    [InlineData("id", "asc")]
    [InlineData("id", "desc")]
    [InlineData("createdat", "asc")]
    [InlineData("createdat", "desc")]
    [InlineData("", "desc")]
    public async Task SearchCastMember_WhenReceivesValidSearchInput_ReturnOrderedList(
        string orderBy,
        string direction)
    {
        var serviceProvider = _fixture.ServiceProvider;
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        var elasticClient = _fixture.ElasticClient;
        var examples = _fixture.GetCastMemberModelList();
        await elasticClient.IndexManyAsync(examples);
        await elasticClient.Indices.RefreshAsync(ElasticsearchIndices.CastMember);
        var input = new SearchCastMemberInput(
            page: 1,
            perPage: examples.Count,
            orderBy: orderBy,
            order: direction == "asc" ? SearchOrder.Asc : SearchOrder.Desc);
        var expectedList = _fixture.CloneCastMembersListOrdered(
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
            outputItem.Type.Should().Be(expected.Type);
            outputItem.CreatedAt.Should().Be(expected.CreatedAt);
        }
    }

    public void Dispose() => _fixture.DeleteAll();
}