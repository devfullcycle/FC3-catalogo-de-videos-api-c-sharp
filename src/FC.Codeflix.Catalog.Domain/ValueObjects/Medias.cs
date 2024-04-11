namespace FC.Codeflix.Catalog.Domain.ValueObjects;

public class Medias
{
    public string? ThumbUrl { get; private set; }
    public string? ThumbHalfUrl { get; private set; }
    public string? BannerUrl { get; private set; }
    public string? MediaUrl { get; private set; }
    public string? TrailerUrl { get; private set; }

    public Medias(
        string? thumbUrl,
        string? thumbHalfUrl,
        string? bannerUrl,
        string? mediaUrl,
        string? trailerUrl)
    {
        ThumbUrl = thumbUrl;
        ThumbHalfUrl = thumbHalfUrl;
        BannerUrl = bannerUrl;
        MediaUrl = mediaUrl;
        TrailerUrl = trailerUrl;
    }
}