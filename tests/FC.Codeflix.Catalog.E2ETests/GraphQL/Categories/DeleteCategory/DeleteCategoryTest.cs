using FC.Codeflix.Catalog.E2ETests.GraphQL.Categories.Common;
using FC.Codeflix.Catalog.Infra.Data.ES.Models;
using FluentAssertions;
using Nest;

namespace FC.Codeflix.Catalog.E2ETests.GraphQL.Categories.DeleteCategory;
[Collection(nameof(CategoryTestFixture))]
public class DeleteCategoryTest : IDisposable
{
    private readonly CategoryTestFixture _fixture;

    public DeleteCategoryTest(CategoryTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(DeleteCategory_WhenReceivesAnExistingId_DeletesCategory))]
    [Trait("E2E/GraphQL", "[Category] Delete")]
    public async Task DeleteCategory_WhenReceivesAnExistingId_DeletesCategory()
    {
        var elasticClient = _fixture.ElasticClient;
        var categoriesExample = _fixture.GetCategoryModelList();
        await elasticClient.IndexManyAsync(categoriesExample);
        var id = categoriesExample[3].Id;

        var output = await _fixture.GraphQLClient.DeleteCategoryOperation
            .ExecuteAsync(id, CancellationToken.None);

        output.Data!.Should().NotBeNull();
        output.Data!.DeleteCategory.Should().BeTrue();
        var deletedCategory = await elasticClient.GetAsync<CategoryModel>(id);
        deletedCategory.Found.Should().BeFalse();
    }

    [Fact(DisplayName = nameof(DeleteCategory_WhenReceivesANonExistingId_ReturnsErrors))]
    [Trait("E2E/GraphQL", "[Category] Delete")]
    public async Task DeleteCategory_WhenReceivesANonExistingId_ReturnsErrors()
    {
        var elasticClient = _fixture.ElasticClient;
        var categoriesExample = _fixture.GetCategoryModelList();
        await elasticClient.IndexManyAsync(categoriesExample);
        var id = Guid.NewGuid();
        var expectedErrorMessage = $"Category '{id}' not found.";

        var output = await _fixture.GraphQLClient.DeleteCategoryOperation
            .ExecuteAsync(id, CancellationToken.None);

        output.Data.Should().BeNull();
        output.Errors.Should().NotBeEmpty();
        output.Errors.Single().Message.Should().Be(expectedErrorMessage);
    }

    public void Dispose() => _fixture.DeleteAll();
}
