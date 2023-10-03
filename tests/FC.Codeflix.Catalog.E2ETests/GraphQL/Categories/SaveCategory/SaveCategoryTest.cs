using FC.Codeflix.Catalog.E2ETests.GraphQL.Categories.Common;
using FC.Codeflix.Catalog.Infra.Data.ES.Models;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Nest;

namespace FC.Codeflix.Catalog.E2ETests.GraphQL.Categories.SaveCategory;
[Collection(nameof(SaveCategoryTestFixture))]
public class SaveCategoryTest : IDisposable
{
    private readonly SaveCategoryTestFixture _fixture;

    public SaveCategoryTest(SaveCategoryTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(SaveCategory_WhenInputIsValid_PersitsCategory))]
    [Trait("E2E/GraphQL", "[Category] Save")]
    public async Task SaveCategory_WhenInputIsValid_PersitsCategory()
    {
        var serviceProvider = _fixture.WebAppFactory.Services;
        var elasticClient = serviceProvider.GetRequiredService<IElasticClient>();
        var input =_fixture.GetValidInput();

        var output = await _fixture.GraphQLClient.SaveCategory
            .ExecuteAsync(input, CancellationToken.None);

        var persisted = await elasticClient
            .GetAsync<CategoryModel>(input.Id);
        persisted.Found.Should().BeTrue();
        var document = persisted.Source;
        document.Should().NotBeNull();
        document.Id.Should().Be(input.Id);
        document.Name.Should().Be(input.Name);
        document.Description.Should().Be(input.Description);
        document.IsActive.Should().Be(input.IsActive);
        document.CreatedAt.Should().Be(input.CreatedAt.DateTime);
        output.Should().NotBeNull();
        output.Data!.SaveCategory.Id.Should().Be(input.Id);
        output.Data.SaveCategory.Name.Should().Be(input.Name);
        output.Data.SaveCategory.Description.Should().Be(input.Description);
        output.Data.SaveCategory.IsActive.Should().Be(input.IsActive);
        output.Data.SaveCategory.CreatedAt.Should().Be(input.CreatedAt);
    }

    [Fact(DisplayName = nameof(SaveCategory_WhenInputIsInvalid_ThrowsException))]
    [Trait("E2E/GraphQL", "[Category] Save")]
    public async Task SaveCategory_WhenInputIsInvalid_ThrowsException()
    {
        var serviceProvider = _fixture.WebAppFactory.Services;
        var elasticClient = serviceProvider.GetRequiredService<IElasticClient>();
        var input = _fixture.GetInvalidInput();
        var expectedMessage = "Name should not be empty or null";

        var output = await _fixture.GraphQLClient.SaveCategory
           .ExecuteAsync(input, CancellationToken.None);

        output.Data.Should().BeNull();
        output.Errors.Should().NotBeEmpty();
        output.Errors.Single().Message.Should().Be(expectedMessage);
        var persisted = await elasticClient
            .GetAsync<CategoryModel>(input.Id);
        persisted.Found.Should().BeFalse();

    }

    public void Dispose() => _fixture.DeleteAll();
}