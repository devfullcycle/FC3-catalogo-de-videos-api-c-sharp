using FC.Codeflix.Catalog.UnitTests.Application.UseCases.CastMember.Common;

namespace FC.Codeflix.Catalog.UnitTests.Application.UseCases.CastMember.SaveCastMember;

public class SaveCastMemberUseCaseTestFixture : CastMemberUseCaseTestFixture
{
    public SaveCastMemberInput GetValidInput()
    {
        var castMember = DataGenerator.GetValidCastMember();
        return new SaveCastMemberInput(
            castMember.Id,
            castMember.Name,
            castMember.Type,
            castMember.CreatedAt);
    }
    
    public SaveCastMemberInput GetInvalidInput()
    {
        var castMember = DataGenerator.GetValidCastMember();
        return new SaveCastMemberInput(
            castMember.Id,
            string.Empty,
            castMember.Type,
            castMember.CreatedAt);
    }

}

[CollectionDefinition(nameof(SaveCastMemberUseCaseTestFixture))]
public class SaveCastMemberUseCaseTestFixtureCollection
    : ICollectionFixture<SaveCastMemberUseCaseTestFixture>
{ }