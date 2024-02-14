using FC.Codeflix.Catalog.Domain.Repositories;
using FC.Codeflix.Catalog.Tests.Shared;
using NSubstitute;
using DomainEntity = FC.Codeflix.Catalog.Domain.Entity;

namespace FC.Codeflix.Catalog.UnitTests.Application.UseCases.CastMember.Common;

public class CastMemberUseCaseTestFixture
{
    public CastMemberDataGenerator DataGenerator { get; } = new();

    public ICastMemberRepository GetMockRepository()
        => Substitute.For<ICastMemberRepository>();

    public DomainEntity.CastMember GetValidCastMember()
        => DataGenerator.GetValidCastMember();
}

[CollectionDefinition(nameof(CastMemberUseCaseTestFixture))]
public class CastMemberUseCaseTestFixtureCollection
    : ICollectionFixture<CastMemberUseCaseTestFixture>
{ }