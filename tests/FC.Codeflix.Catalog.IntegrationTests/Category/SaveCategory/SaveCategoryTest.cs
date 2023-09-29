using FC.Codeflix.Catalog.Domain.Exceptions;
using FC.Codeflix.Catalog.Infra.Data.ES.Models;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Nest;

namespace FC.Codeflix.Catalog.IntegrationTests.Category.SaveCategory;
[Collection(nameof(SaveCategoryTestFixture))]
public class SaveCategoryTest : IDisposable
{
    private readonly SaveCategoryTestFixture _fixture;

    public SaveCategoryTest(SaveCategoryTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(SaveCategory_WhenInputIsValid_PersitsCategory))]
    [Trait("Integration", "[UseCase] SaveCategory")]
    public async Task SaveCategory_WhenInputIsValid_PersitsCategory()
    {
        var serviceProvider = _fixture.ServiceProvider;
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        var elasticClient = _fixture.ElasticClient;
        var input = _fixture.GetValidInput();

        var output = await mediator.Send(input);

        var persisted = await elasticClient
            .GetAsync<CategoryModel>(input.Id);
        persisted.Found.Should().BeTrue();
        var document = persisted.Source;
        document.Should().NotBeNull();
        document.Id.Should().Be(input.Id);
        document.Name.Should().Be(input.Name);
        document.Description.Should().Be(input.Description);
        document.IsActive.Should().Be(input.IsActive);
        document.CreatedAt.Should().Be(input.CreatedAt);
        output.Should().NotBeNull();
        output.Id.Should().Be(input.Id);
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().Be(input.IsActive);
        output.CreatedAt.Should().Be(input.CreatedAt);
    }

    [Fact(DisplayName = nameof(SaveCategory_WhenInputIsInvalid_ThrowsException))]
    [Trait("Integration", "[UseCase] SaveCategory")]
    public async Task SaveCategory_WhenInputIsInvalid_ThrowsException()
    {
        var serviceProvider = _fixture.ServiceProvider;
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        var elasticClient = _fixture.ElasticClient;
        var input = _fixture.GetInvalidInput();
        var expectedMessage = "Name should not be empty or null";

        var action = async () => await mediator.Send(input);

        await action.Should().ThrowAsync<EntityValidationException>()
            .WithMessage(expectedMessage);
        var persisted = await elasticClient
            .GetAsync<CategoryModel>(input.Id);
        persisted.Found.Should().BeFalse();
        
    }

    public void Dispose() => _fixture.DeleteAll();
}
