using FC.Codeflix.Catalog.Infra.Data.ES.Models;
using FluentAssertions;
using Nest;

namespace FC.Codeflix.Catalog.E2ETests.Consumers.Category;

[Collection(nameof(CategoryConsumerTestFixture))]
public class CategoryConsumerTest : IDisposable
{
    private readonly CategoryConsumerTestFixture _fixture;

    public CategoryConsumerTest(CategoryConsumerTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Theory(DisplayName = nameof(CategoryEvent_WhenOperationIsCreateOrRead_SavesCategory))]
    [Trait("E2E/Consumers", "Category")]
    [InlineData("c")]
    [InlineData("r")]
    public async Task CategoryEvent_WhenOperationIsCreateOrRead_SavesCategory(string operation)
    {
        var message = _fixture.BuildValidMessage(operation);
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
    
    [Fact(DisplayName = nameof(CategoryEvent_WhenOperationIsUpdate_SavesCategory))]
    [Trait("E2E/Consumers", "Category")]
    public async Task CategoryEvent_WhenOperationIsUpdate_SavesCategory()
    {
        var examplesList = _fixture.GetCategoryModelList();
        await _fixture.ElasticClient.IndexManyAsync(examplesList);
        var example = examplesList[2];
        var message = _fixture.BuildValidMessage("u", example);
        var category = message.Payload.After;
        category.Name = _fixture.DataGenerator.GetValidCategoryName();
        
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
    
    [Fact(DisplayName = nameof(CategoryEvent_WhenOperationIsDelete_DeletesCategory))]
    [Trait("E2E/Consumers", "Category")]
    public async Task CategoryEvent_WhenOperationIsDelete_DeletesCategory()
    {
        var examplesList = _fixture.GetCategoryModelList();
        await _fixture.ElasticClient.IndexManyAsync(examplesList);
        var example = examplesList[2];
        var message = _fixture.BuildValidMessage("d", example);
        var category = message.Payload.Before;
        
        await _fixture.PublishMessageAsync(message);
        await Task.Delay(2_000);
        
        var persisted = await _fixture.ElasticClient
            .GetAsync<CategoryModel>(category.Id);
        persisted.Found.Should().BeFalse();
    }

    public void Dispose() => _fixture.DeleteAll();
}