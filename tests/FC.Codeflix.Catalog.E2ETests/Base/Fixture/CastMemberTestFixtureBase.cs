using FC.Codeflix.Catalog.Infra.Data.ES.Models;
using FC.Codeflix.Catalog.Tests.Shared;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Nest;

namespace FC.Codeflix.Catalog.E2ETests.Base.Fixture;

public class CastMemberTestFixtureBase : FixtureBase, IDisposable
{
    public CustomWebApplicationFactory<Program> WebAppFactory { get; }
    public IElasticClient ElasticClient { get; }
    public CastMemberDataGenerator DataGenerator { get; }
    
    protected CastMemberTestFixtureBase()
    {
        WebAppFactory = new CustomWebApplicationFactory<Program>();
        _ = WebAppFactory.CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri(WebAppFactory.BaseUrl)
        });
        DataGenerator = new CastMemberDataGenerator();
        ElasticClient = WebAppFactory.Services.GetRequiredService<IElasticClient>();   
        ElasticClient.CreateCastMemberIndexAsync().GetAwaiter().GetResult();
    }

    public IList<CastMemberModel> GetCastMemberModelList(int count = 10)
        => DataGenerator.GetCastMemberModelList(count);

    public void DeleteAll()
        => ElasticClient.DeleteDocuments<CastMemberModel>();

    public void Dispose()
        => ElasticClient.DeleteCastMemberIndex();
}