using FC.Codeflix.Catalog.Tests.Shared;

namespace FC.Codeflix.Catalog.UnitTests.Domain.Entity.CastMember;

public class CastMemberTestFixture
{
    private readonly CastMemberDataGenerator _dataGenerator = new();
    
    public Catalog.Domain.Entity.CastMember GetValidCastMember()
        => _dataGenerator.GetValidCastMember();
}

[CollectionDefinition(nameof(CastMemberTestFixture))]
public class CastMemberTestFixtureCollection
    : ICollectionFixture<CastMemberTestFixture>
{ }