using FC.Codeflix.Catalog.Infra.Data.ES.Models;
using FluentAssertions;
using Nest;

namespace FC.Codeflix.Catalog.E2ETests.Consumers.CastMember;

[Collection(nameof(CastMemberConsumerTestFixture))]
public class CastMemberConsumerTest : IDisposable
{
    private readonly CastMemberConsumerTestFixture _fixture;

    public CastMemberConsumerTest(CastMemberConsumerTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Theory(DisplayName = nameof(CastMemberEvent_WhenOperationIsCreateOrRead_SavesCastMember))]
    [Trait("E2E/Consumers", "CastMember")]
    [InlineData("c")]
    [InlineData("r")]
    public async Task CastMemberEvent_WhenOperationIsCreateOrRead_SavesCastMember(string operation)
    {
        var message = _fixture.BuildValidMessage(operation);
        var castMember = message.Payload.After;
        
        await _fixture.PublishMessageAsync(message);
        await Task.Delay(2_000);
        
        var persisted = await _fixture.ElasticClient
            .GetAsync<CastMemberModel>(castMember.Id);
        persisted.Found.Should().BeTrue();
        var document = persisted.Source;
        document.Should().NotBeNull();
        document.Id.Should().Be(castMember.Id);
        document.Name.Should().Be(castMember.Name);
        document.Type.Should().Be(castMember.Type);
        document.CreatedAt.Date.Should().Be(castMember.CreatedAt.Date);
    }
    
    [Fact(DisplayName = nameof(CastMemberEvent_WhenOperationIsUpdate_SavesCastMember))]
    [Trait("E2E/Consumers", "CastMember")]
    public async Task CastMemberEvent_WhenOperationIsUpdate_SavesCastMember()
    {
        var examplesList = _fixture.GetCastMemberModelList();
        await _fixture.ElasticClient.IndexManyAsync(examplesList);
        var example = examplesList[2];
        var message = _fixture.BuildValidMessage("u", example);
        var castMember = message.Payload.After;
        castMember.Name = _fixture.DataGenerator.GetValidCastMemberName();
        
        await _fixture.PublishMessageAsync(message);
        await Task.Delay(2_000);
        
        var persisted = await _fixture.ElasticClient
            .GetAsync<CastMemberModel>(castMember.Id);
        persisted.Found.Should().BeTrue();
        var document = persisted.Source;
        document.Should().NotBeNull();
        document.Id.Should().Be(castMember.Id);
        document.Name.Should().Be(castMember.Name);
        document.Type.Should().Be(castMember.Type);
        document.CreatedAt.Date.Should().Be(castMember.CreatedAt.Date);
    }
    
    [Fact(DisplayName = nameof(CastMemberEvent_WhenOperationIsDelete_DeletesCastMember))]
    [Trait("E2E/Consumers", "CastMember")]
    public async Task CastMemberEvent_WhenOperationIsDelete_DeletesCastMember()
    {
        var examplesList = _fixture.GetCastMemberModelList();
        await _fixture.ElasticClient.IndexManyAsync(examplesList);
        var example = examplesList[2];
        var message = _fixture.BuildValidMessage("d", example);
        var castMember = message.Payload.Before;
        
        await _fixture.PublishMessageAsync(message);
        await Task.Delay(2_000);
        
        var persisted = await _fixture.ElasticClient
            .GetAsync<CastMemberModel>(castMember.Id);
        persisted.Found.Should().BeFalse();
    }

    public void Dispose() => _fixture.DeleteAll();
}