using FC.Codeflix.Catalog.Domain.Repositories.DTOs;
using FC.Codeflix.Catalog.Infra.Data.ES.Models;
using FC.Codeflix.Catalog.IntegrationTests.CastMember.Common;

namespace FC.Codeflix.Catalog.IntegrationTests.CastMember.SearchCastMember;

public class SearchCastMemberTestFixture : CastMemberTestFixture
{
    public IList<CastMemberModel> GetCastMemberModelList(IEnumerable<string> castMemberNames)
        => DataGenerator.GetCastMemberModelList(castMemberNames);

    public IList<CastMemberModel> CloneCastMembersListOrdered(
        IList<CastMemberModel> castMembersList,
        string orderBy,
        SearchOrder direction)
        => DataGenerator.CloneCastMembersListOrdered(castMembersList, orderBy, direction);
}

[CollectionDefinition(nameof(SearchCastMemberTestFixture))]
public class SearchCastMemberTestFixtureCollection
    : ICollectionFixture<SearchCastMemberTestFixture>
{ }