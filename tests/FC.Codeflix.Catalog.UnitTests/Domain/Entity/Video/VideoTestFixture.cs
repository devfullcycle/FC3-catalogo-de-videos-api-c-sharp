using FC.Codeflix.Catalog.Tests.Shared;

namespace FC.Codeflix.Catalog.UnitTests.Domain.Entity.Video;

public class VideoTestFixture
{
    private readonly VideoDataGenerator _dataGenerator = new();
    private readonly CategoryDataGenerator _categoryDataGenerator = new();
    private readonly GenreDataGenerator _genreDataGenerator = new();
    private readonly CastMemberDataGenerator _castMemberDataGenerator = new();
    
    public Catalog.Domain.Entity.Video GetValidVideo()
        => _dataGenerator.GetValidVideo();
}

[CollectionDefinition(nameof(VideoTestFixture))]
public class VideoTestFixtureCollection
    : ICollectionFixture<VideoTestFixture>
{ }