using FC.Codeflix.Catalog.Application.UseCases.Category.SearchCategory;
using FC.Codeflix.Catalog.Infra.Data.ES;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FC.Codeflix.Catalog.IntegrationTests.Category.SearchCategory;
[Collection(nameof(SearchCategoryTestFixture))]
public class SearchCategoryTest : IDisposable
{
    private readonly SearchCategoryTestFixture _fixture;

    public SearchCategoryTest(SearchCategoryTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Theory(DisplayName = nameof(SearchCategory_WhenReceivesValidSearchInput_ReturnFilteredList))]
    [Trait("Integration", "[UseCase] SearchCategory")]
    [InlineData("Action", 1, 5, 1, 1)]
    [InlineData("Horror", 1, 5, 3, 3)]
    [InlineData("Horror", 2, 5, 0, 3)]
    [InlineData("Sci-fi", 1, 5, 4, 4)]
    [InlineData("Sci-fi", 1, 2, 2, 4)]
    [InlineData("Sci-fi", 2, 3, 1, 4)]
    [InlineData("Others", 1, 5, 0, 0)]
    [InlineData("Robots", 1, 5, 1, 1)]
    public async Task SearchCategory_WhenReceivesValidSearchInput_ReturnFilteredList(
        string search,
        int page,
        int perPage,
        int expectedItemsCount,
        int expectedTotalCount)
    {
        var serviceProvider = _fixture.ServiceProvider;
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        var elasticClient = serviceProvider.GetRequiredService<IElasticClient>();
        var categoryNamesList = new List<string>() {
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
        var examples = _fixture.GetCategoryModelList(categoryNamesList);
        await elasticClient.IndexDocumentAsync(examples);
        await elasticClient.Indices.RefreshAsync(ElasticsearchIndices.Category);
        var input = new SearchCategoryInput(page: page, perPage: perPage, search: search);

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
            outputItem.Description.Should().Be(expected.Description);
            outputItem.IsActive.Should().Be(expected.IsActive);
            outputItem.CreatedAt.Should().Be(expected.CreatedAt);
        }

    }

    public void Dispose() => _fixture.DeleteAll();
}
