using FC.Codeflix.Catalog.Application.UseCases.CastMember.DeleteCastMember;
using FC.Codeflix.Catalog.Domain.Exceptions;
using FC.Codeflix.Catalog.Infra.Data.ES.Models;
using FC.Codeflix.Catalog.IntegrationTests.CastMember.Common;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Nest;

namespace FC.Codeflix.Catalog.IntegrationTests.CastMember.DeleteCastMember;

[Collection(nameof(CastMemberTestFixture))]
public class DeleteCastMemberTest : IDisposable
{
    private readonly CastMemberTestFixture _fixture;

    public DeleteCastMemberTest(CastMemberTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(DeleteCastMember_WhenReceivesAnExistingId_DeletesCastMember))]
    [Trait("Integration", "[UseCase] DeleteCastMember")]
    public async Task DeleteCastMember_WhenReceivesAnExistingId_DeletesCastMember()
    {
        var serviceProvider = _fixture.ServiceProvider;
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        var elasticClient = _fixture.ElasticClient;
        var castMembersExample = _fixture.GetCastMemberModelList();
        await elasticClient.IndexManyAsync(castMembersExample);
        var input = new DeleteCastMemberInput(castMembersExample[3].Id);

        await mediator.Send(input, CancellationToken.None);

        var deletedCastMember = await elasticClient.GetAsync<CastMemberModel>(input.Id);
        deletedCastMember.Found.Should().BeFalse();
    }

    [Fact(DisplayName = nameof(DeleteCastMember_WhenReceivesANonExistingId_ThrowsException))]
    [Trait("Integration", "[UseCase] DeleteCastMember")]
    public async Task DeleteCastMember_WhenReceivesANonExistingId_ThrowsException()
    {
        var serviceProvider = _fixture.ServiceProvider;
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        var elasticClient = _fixture.ElasticClient;
        var castMembersExample = _fixture.GetCastMemberModelList();
        await elasticClient.IndexManyAsync(castMembersExample);
        var input = new DeleteCastMemberInput(Guid.NewGuid());

        var action = async () => await mediator.Send(input, CancellationToken.None);

        await action.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"CastMember '{input.Id}' not found.");
    }

    public void Dispose() => _fixture.DeleteAll();
}