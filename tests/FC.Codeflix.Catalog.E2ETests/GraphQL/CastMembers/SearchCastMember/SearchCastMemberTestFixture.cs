using FC.Codeflix.Catalog.E2ETests.Base.Fixture;
using FC.Codeflix.Catalog.Infra.Data.ES.Models;
using RepositoryDTOs = FC.Codeflix.Catalog.Domain.Repositories.DTOs;

namespace FC.Codeflix.Catalog.E2ETests.GraphQL.CastMembers.SearchCastMember;

public class SearchCastMemberTestFixture : CastMemberTestFixtureBase
{
    public CatalogClient GraphQLClient { get; }
    public SearchCastMemberTestFixture()
    {
        GraphQLClient = WebAppFactory.GraphQLClient;
    }
    
    public IList<CastMemberModel> GetCastMemberModelList(IEnumerable<string> castMemberNames)
        => DataGenerator.GetCastMemberModelList(castMemberNames);

    public IList<CastMemberModel> CloneCastMembersListOrdered(
        IList<CastMemberModel> castMembersList,
        string orderBy,
        RepositoryDTOs.SearchOrder direction)
        => DataGenerator.CloneCastMembersListOrdered(castMembersList, orderBy, direction);
}

[CollectionDefinition(nameof(SearchCastMemberTestFixture))]
public class SearchCastMemberTestFixtureCollection
    : ICollectionFixture<SearchCastMemberTestFixture>
{ }
