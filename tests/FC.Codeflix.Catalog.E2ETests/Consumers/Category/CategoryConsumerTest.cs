using FC.Codeflix.Catalog.Infra.Data.ES.Models;
using FluentAssertions;

namespace FC.Codeflix.Catalog.E2ETests.Consumers.Category;

[Collection(nameof(CategoryConsumerTestFixture))]
public class CategoryConsumerTest : IDisposable
{
    private readonly CategoryConsumerTestFixture _fixture;

    public CategoryConsumerTest(CategoryConsumerTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(CategoryEvent_WhenOperationIsCreate_SavesCategory))]
    [Trait("E2E/Consumers", "Category")]
    public async Task CategoryEvent_WhenOperationIsCreate_SavesCategory()
    {
        var message = _fixture.BuildValidMessage("c");
        var category = message.Payload.After;
        
        await _fixture.PublishMessageAsync(message);
        await Task.Delay(2_000);
        
        var persisted = await _fixture.ElasticClient
            .GetAsync<CategoryModel>(category.Id);
        persisted.Found.Should().BeTrue();
        var document = persisted.Source;
        document.Should().NotBeNull();
        document.Id.Should().Be(category.Id);
        document.Name.Should().Be(category.Name);
        document.Description.Should().Be(category.Description);
        document.IsActive.Should().Be(category.IsActive);
        document.CreatedAt.Date.Should().Be(category.CreatedAt.Date);
    }

    public void Dispose() => _fixture.DeleteAll();
}