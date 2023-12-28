using FC.Codeflix.Catalog.Infra.Data.ES.Models;
using FC.Codeflix.Catalog.IntegrationTests.Common;
using FC.Codeflix.Catalog.Tests.Shared;
using Microsoft.Extensions.DependencyInjection;
using Nest;

namespace FC.Codeflix.Catalog.IntegrationTests.Genre.Common;

public class GenreTestFixture : BaseFixture, IDisposable
{
    public IElasticClient ElasticClient { get; }
    public GenreDataGenerator DataGenerator { get; }
    
    public GenreTestFixture()
    {
        ElasticClient = ServiceProvider.GetRequiredService<IElasticClient>();
        DataGenerator = new GenreDataGenerator();
        ElasticClient.CreateGenreIndexAsync().GetAwaiter().GetResult();
    }
    
    public void DeleteAll()
        => ElasticClient.DeleteDocuments<GenreModel>();

    public List<GenreModel> GetGenreModelList(int count = 10)
        => DataGenerator.GetGenreModelList(count);

    public void Dispose()
        => ElasticClient.DeleteGenreIndex();
}

[CollectionDefinition(nameof(GenreTestFixture))]
public class GenreTestFixtureCollection 
    : ICollectionFixture<GenreTestFixture>
{ }