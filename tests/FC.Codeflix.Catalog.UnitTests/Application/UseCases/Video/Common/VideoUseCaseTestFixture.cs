using FC.Codeflix.Catalog.Domain.Gateways;
using FC.Codeflix.Catalog.Domain.Repositories;
using FC.Codeflix.Catalog.Tests.Shared;
using NSubstitute;
using DomainEntity = FC.Codeflix.Catalog.Domain.Entity;

namespace FC.Codeflix.Catalog.UnitTests.Application.UseCases.Video.Common;

public class VideoUseCaseTestFixture
{
    public VideoDataGenerator DataGenerator { get; } = new();

    public IVideoRepository GetMockRepository()
        => Substitute.For<IVideoRepository>();
    
    public IAdminCatalogGateway GetMockAdminCatalogGateway()
        => Substitute.For<IAdminCatalogGateway>();

    public DomainEntity.Video GetValidVideo()
        => DataGenerator.GetValidVideo();

    public IList<DomainEntity.Video> GetVideoList(int count = 10)
        => DataGenerator.GetVideoList(count);
}

[CollectionDefinition(nameof(VideoUseCaseTestFixture))]
public class VideoUseCaseTestFixtureCollection
    : ICollectionFixture<VideoUseCaseTestFixture>
{ }