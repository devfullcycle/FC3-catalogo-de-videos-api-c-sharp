using FC.Codeflix.Catalog.Application.UseCases.Category.DeleteCategory;
using FC.Codeflix.Catalog.Domain.Exceptions;
using FC.Codeflix.Catalog.Infra.Data.ES.Models;
using FC.Codeflix.Catalog.IntegrationTests.Category.Common;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Nest;

namespace FC.Codeflix.Catalog.IntegrationTests.Category.DeleteCategory;
[Collection(nameof(CategoryTestFixture))]
public class DeleteCategoryTest : IDisposable
{
    private readonly CategoryTestFixture _fixture;

    public DeleteCategoryTest(CategoryTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(DeleteCategory_WhenReceivesAnExistingId_DeletesCategory))]
    [Trait("Integration", "[UseCase] DeleteCategory")]
    public async Task DeleteCategory_WhenReceivesAnExistingId_DeletesCategory()
    {
        var serviceProvider = _fixture.ServiceProvider;
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        var elasticClient = _fixture.ElasticClient;
        var categoriesExample = _fixture.GetCategoryModelList();
        await elasticClient.IndexManyAsync(categoriesExample);
        var input = new DeleteCategoryInput(categoriesExample[3].Id);

        await mediator.Send(input, CancellationToken.None);

        var deletedCategory = await elasticClient.GetAsync<CategoryModel>(input.Id);
        deletedCategory.Found.Should().BeFalse();
    }

    [Fact(DisplayName = nameof(DeleteCategory_WhenReceivesANonExistingId_ThrowsException))]
    [Trait("Integration", "[UseCase] DeleteCategory")]
    public async Task DeleteCategory_WhenReceivesANonExistingId_ThrowsException()
    {
        var serviceProvider = _fixture.ServiceProvider;
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        var elasticClient = _fixture.ElasticClient;
        var categoriesExample = _fixture.GetCategoryModelList();
        await elasticClient.IndexManyAsync(categoriesExample);
        var input = new DeleteCategoryInput(Guid.NewGuid());

        var action = async () => await mediator.Send(input, CancellationToken.None);

        await action.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Category '{input.Id}' not found.");
    }

    public void Dispose() => _fixture.DeleteAll();
}
