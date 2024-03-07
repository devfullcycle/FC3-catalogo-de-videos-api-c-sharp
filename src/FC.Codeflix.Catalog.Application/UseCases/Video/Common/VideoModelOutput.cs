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
    public IReadOnlyList<VideoModelOutputCategory> Categories { get; private set; } = null!;
    public IReadOnlyList<VideoModelOutputGenre> Genres { get; private set; } = null!;
    public IReadOnlyList<VideoModelOutputCastMember> CastMembers { get; private set; } = null!;
    
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
                .Select(category => new VideoModelOutputCategory(category.Id, category.Name))
                .ToList()
                .AsReadOnly(),
            Genres = video.Genres
                .Select(genre => new VideoModelOutputGenre(genre.Id, genre.Name))
                .ToList()
                .AsReadOnly(),
            CastMembers = video.CastMembers
                .Select(castMember => new VideoModelOutputCastMember(castMember.Id, castMember.Name, castMember.Type))
                .ToList()
                .AsReadOnly()
        };

        return output;
    }
}

public class VideoModelOutputCategory
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;
    
    public VideoModelOutputCategory(Guid id, string name)
    {
        Id = id;
        Name = name;
    }
}

public class VideoModelOutputGenre
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;
    
    public VideoModelOutputGenre(Guid id, string name)
    {
        Id = id;
        Name = name;
    }
}

public class VideoModelOutputCastMember
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public CastMemberType Type { get; private set; }
    
    public VideoModelOutputCastMember(Guid id, string name, CastMemberType type)
    {
        Id = id;
        Name = name;
        Type = type;
    }
}