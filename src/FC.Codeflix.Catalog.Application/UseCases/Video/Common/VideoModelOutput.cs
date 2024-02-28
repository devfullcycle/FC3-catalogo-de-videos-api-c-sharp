using FC.Codeflix.Catalog.Domain.Enums;
using DomainEntity = FC.Codeflix.Catalog.Domain.Entity;

namespace FC.Codeflix.Catalog.Application.UseCases.Video.Common;

public class VideoModelOutput
{
    public Guid Id { get; private set; }
    public string Title { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public int YearLaunched { get; private set; }
    public int Duration { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public Rating Rating { get; private set; }
    public string? ThumbUrl { get; private set; }
    public string? ThumbHalfUrl { get; private set; }
    public string? BannerUrl { get; private set; }
    public string? MediaUrl { get; private set; }
    public string? TrailerUrl { get; private set; }
    public IReadOnlyList<VideoModelOutputRelation> Categories { get; private set; } = null!;
    public IReadOnlyList<VideoModelOutputRelation> Genres { get; private set; } = null!;
    public IReadOnlyList<VideoModelOutputRelation> CastMembers { get; private set; } = null!;
    
    public static VideoModelOutput FromVideo(
        DomainEntity.Video video)
    {
        var output = new VideoModelOutput
        {
            Id = video.Id,
            Title = video.Title,
            Description = video.Description,
            YearLaunched = video.YearLaunched,
            Duration = video.Duration,
            CreatedAt = video.CreatedAt,
            Rating = video.Rating,
            ThumbUrl = video.Medias?.ThumbUrl,
            ThumbHalfUrl = video.Medias?.ThumbHalfUrl,
            BannerUrl = video.Medias?.BannerUrl,
            MediaUrl = video.Medias?.MediaUrl,
            TrailerUrl = video.Medias?.TrailerUrl,
            Categories = video.Categories
                .Select(category => new VideoModelOutputRelation(category.Id, category.Name))
                .ToList()
                .AsReadOnly(),
            Genres = video.Genres
                .Select(genre => new VideoModelOutputRelation(genre.Id, genre.Name))
                .ToList()
                .AsReadOnly(),
            CastMembers = video.CastMembers
                .Select(castMember => new VideoModelOutputRelation(castMember.Id, castMember.Name))
                .ToList()
                .AsReadOnly()
        };

        return output;
    }
}

public class VideoModelOutputRelation
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;
    
    public VideoModelOutputRelation(Guid id, string name)
    {
        Id = id;
        Name = name;
    }
}