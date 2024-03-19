using FC.Codeflix.Catalog.Api.Common;
using FC.Codeflix.Catalog.Application.Common;
using FC.Codeflix.Catalog.Application.UseCases.Video.Common;
using FC.Codeflix.Catalog.Domain.Enums;
using FC.Codeflix.Catalog.Domain.Repositories.DTOs;
using MediatR;

namespace FC.Codeflix.Catalog.Api.Videos;

public class VideoPayload
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int YearLaunched { get; set; }
    public int Duration { get; set; }
    public DateTime CreatedAt { get; set; }
    public Rating Rating { get; set; }
    public string? ThumbUrl { get; set; }
    public string? ThumbHalfUrl { get; set; }
    public string? BannerUrl { get; set; }
    public string? MediaUrl { get; set; }
    public string? TrailerUrl { get; set; }
    public IReadOnlyList<VideoCategoryPayload> Categories { get; set; } = null!;
    public IReadOnlyList<VideoGenrePayload> Genres { get; set; } = null!;
    public IReadOnlyList<VideoCastMemberPayload> CastMembers { get; set; } = null!;
    
    public static VideoPayload FromVideoModelOutput(VideoModelOutput output)
    {
        return new VideoPayload
        {
            Id = output.Id,
            Title = output.Title,
            Description = output.Description,
            YearLaunched = output.YearLaunched,
            Duration = output.Duration,
            CreatedAt = output.CreatedAt,
            Rating = output.Rating,
            ThumbUrl = output.ThumbUrl,
            ThumbHalfUrl = output.ThumbHalfUrl,
            BannerUrl = output.BannerUrl,
            MediaUrl = output.MediaUrl,
            TrailerUrl = output.TrailerUrl,
            Categories = output.Categories
                .Select(category => new VideoCategoryPayload(category.Id, category.Name))
                .ToList()
                .AsReadOnly(),
            Genres = output.Genres
                .Select(genre => new VideoGenrePayload(genre.Id, genre.Name))
                .ToList()
                .AsReadOnly(),
            CastMembers = output.CastMembers
                .Select(castMember => new VideoCastMemberPayload(castMember.Id, castMember.Name, castMember.Type))
                .ToList()
                .AsReadOnly()
        };
    }
}

public class VideoCategoryPayload
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    public VideoCategoryPayload(Guid id, string name)
    {
        Id = id;
        Name = name;
    }
}

public class VideoGenrePayload
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    public VideoGenrePayload(Guid id, string name)
    {
        Id = id;
        Name = name;
    }
}

public class VideoCastMemberPayload
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public CastMemberType Type { get; set; }

    public VideoCastMemberPayload(Guid id, string name, CastMemberType type)
    {
        Id = id;
        Name = name;
        Type = type;
    }
}
