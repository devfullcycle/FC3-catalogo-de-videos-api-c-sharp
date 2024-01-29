using FC.Codeflix.Catalog.IntegrationTests.Common;
using FC.Codeflix.Catalog.Tests.Shared;
using Microsoft.Extensions.DependencyInjection;
using Nest;

namespace FC.Codeflix.Catalog.IntegrationTests.CastMember.Common;

public class CastMemberTestFixture : BaseFixture, IDisposable
{
    public CastMemberDataGenerator DataGenerator { get; }
    public IElasticClient ElasticClient { get; }

    public CastMemberTestFixture()
        : base()
    {
        ElasticClient = ServiceProvider.GetRequiredService<IElasticClient>();
        DataGenerator = new CastMemberDataGenerator();
        ElasticClient.CreateCastMemberIndexAsync().GetAwaiter().GetResult();
    }

    public IList<CastMemberModel> GetCastMemberModelList(int count = 10)
        => DataGenerator.GetCastMemberModelList(count);

    public void DeleteAll()
        => ElasticClient.DeleteDocuments<CastMemberModel>();

    public void Dispose()
        => ElasticClient.DeleteCastMemberIndex();
}

[CollectionDefinition(nameof(CastMemberTestFixture))]
public class CastMemberTestFixtureCollection
    : ICollectionFixture<CastMemberTestFixture>
{ }