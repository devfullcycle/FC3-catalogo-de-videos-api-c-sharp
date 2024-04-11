using FC.Codeflix.Catalog.Application.UseCases.CastMember.SaveCastMember;
using FC.Codeflix.Catalog.IntegrationTests.CastMember.Common;

namespace FC.Codeflix.Catalog.IntegrationTests.CastMember.SaveCastMember;

public class SaveCastMemberTestFixture : CastMemberTestFixture
{
    public SaveCastMemberInput GetValidInput()
        => new(
            Guid.NewGuid(),
            DataGenerator.GetValidCastMemberName(),
            DataGenerator.GetRandomCastMemberType(),
            DateTime.Now);

    public SaveCastMemberInput GetInvalidInput()
        => new(
            Guid.NewGuid(),
            null!,
            DataGenerator.GetRandomCastMemberType(),
            DateTime.Now);
}

[CollectionDefinition(nameof(SaveCastMemberTestFixture))]
public class SaveCastMemberTestFixtureCollection
    : ICollectionFixture<SaveCastMemberTestFixture>
{ }