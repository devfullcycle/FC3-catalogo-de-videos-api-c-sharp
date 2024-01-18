using FC.Codeflix.Catalog.Domain.Exceptions;
using FluentAssertions;
using NSubstitute;
using UseCase = FC.Codeflix.Catalog.Application.UseCases.CastMember.SaveCastMember; 
using DomainEntity = FC.Codeflix.Catalog.Domain.Entity;

namespace FC.Codeflix.Catalog.UnitTests.Application.UseCases.CastMember.SaveCastMember;

[Collection(nameof(SaveCastMemberUseCaseTestFixture))]
public class SaveCastMemberUseCaseTest
{
    private readonly SaveCastMemberUseCaseTestFixture _fixture;

    public SaveCastMemberUseCaseTest(SaveCastMemberUseCaseTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(SaveValidCastMember))]
    [Trait("Application", "[UseCase] SaveCastMember")]
    public async Task SaveValidCastMember()
    {
        var repository = _fixture.GetMockRepository();
        var useCase = new UseCase.SaveCastMember(repository);
        var input = _fixture.GetValidInput();

        var output = await useCase.Handle(input, CancellationToken.None);

        await repository.Received(1).SaveAsync(
            Arg.Any<DomainEntity.CastMember>(),
            Arg.Any<CancellationToken>());
        output.Should().NotBeNull();
        output.Id.Should().Be(input.Id);
        output.Name.Should().Be(input.Name);
        output.Type.Should().Be(input.Type);
        output.CreatedAt.Should().Be(input.CreatedAt);
    }

    [Fact(DisplayName = nameof(SaveInvalidCastMember))]
    [Trait("Application", "[UseCase] SaveCastMember")]
    public async Task SaveInvalidCastMember()
    {
        var repository = _fixture.GetMockRepository();
        var useCase = new UseCase.SaveCastMember(repository);
        var input = _fixture.GetInvalidInput();

        var action = async () => await useCase.Handle(input, CancellationToken.None);

        await repository.DidNotReceive().SaveAsync(
            Arg.Any<DomainEntity.CastMember>(),
            Arg.Any<CancellationToken>());
        await action
            .Should()
            .ThrowAsync<EntityValidationException>()
            .WithMessage("Name should not be empty or null");
    }
}