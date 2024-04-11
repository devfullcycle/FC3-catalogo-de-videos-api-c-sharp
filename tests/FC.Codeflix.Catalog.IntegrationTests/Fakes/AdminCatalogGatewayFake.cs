using FC.Codeflix.Catalog.Domain.Entity;
using FC.Codeflix.Catalog.Domain.Gateways;
using FC.Codeflix.Catalog.Tests.Shared;

namespace FC.Codeflix.Catalog.IntegrationTests.Fakes;

public class AdminCatalogGatewayFake
    : IAdminCatalogGateway
{
    private readonly GenreDataGenerator _genreDataGenerator = new();
    private readonly CategoryDataGenerator _categoryDataGenerator = new();
    private readonly CastMemberDataGenerator _castMemberDataGenerator = new();
    private readonly VideoDataGenerator _videoDataGenerator = new();
    public Task<Domain.Entity.Genre> GetGenreAsync(Guid id, CancellationToken cancellationToken)
    {
        return Task.FromResult(_genreDataGenerator.GetValidGenre(id));
    }

    public Task<Domain.Entity.Video> GetVideoAsync(Guid id, CancellationToken cancellationToken)
    {
        var video = _videoDataGenerator.GetValidVideo(id);
        video.AddCategories(_categoryDataGenerator.GetValidCategory());
        video.AddGenres(_genreDataGenerator.GetValidGenre());
        video.AddCastMembers(_castMemberDataGenerator.GetValidCastMember());
        return Task.FromResult(video);
    }
}