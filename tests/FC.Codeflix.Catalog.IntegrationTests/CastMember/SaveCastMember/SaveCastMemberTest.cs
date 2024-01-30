using FC.Codeflix.Catalog.Domain.Exceptions;
using FC.Codeflix.Catalog.Infra.Data.ES.Models;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FC.Codeflix.Catalog.IntegrationTests.CastMember.SaveCastMember;

[Collection(nameof(SaveCastMemberTestFixture))]
public class SaveCastMemberTest : IDisposable
{
    private readonly SaveCastMemberTestFixture _fixture;

    public SaveCastMemberTest(SaveCastMemberTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(SaveCastMember_WhenInputIsValid_PersitsCastMember))]
    [Trait("Integration", "[UseCase] SaveCastMember")]
    public async Task SaveCastMember_WhenInputIsValid_PersitsCastMember()
    {
        var serviceProvider = _fixture.ServiceProvider;
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        var elasticClient = _fixture.ElasticClient;
        var input = _fixture.GetValidInput();

        var output = await mediator.Send(input);

        var persisted = await elasticClient
            .GetAsync<CastMemberModel>(input.Id);
        persisted.Found.Should().BeTrue();
        var document = persisted.Source;
        document.Should().NotBeNull();
        document.Id.Should().Be(input.Id);
        document.Name.Should().Be(input.Name);
        document.Type.Should().Be(input.Type);
        document.CreatedAt.Should().Be(input.CreatedAt);
        output.Should().NotBeNull();
        output.Id.Should().Be(input.Id);
        output.Name.Should().Be(input.Name);
        output.Type.Should().Be(input.Type);
        output.CreatedAt.Should().Be(input.CreatedAt);
    }

    [Fact(DisplayName = nameof(SaveCastMember_WhenInputIsInvalid_ThrowsException))]
    [Trait("Integration", "[UseCase] SaveCastMember")]
    public async Task SaveCastMember_WhenInputIsInvalid_ThrowsException()
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
            .GetAsync<CastMemberModel>(input.Id);
        persisted.Found.Should().BeFalse();
        
    }

    public void Dispose() => _fixture.DeleteAll();
}