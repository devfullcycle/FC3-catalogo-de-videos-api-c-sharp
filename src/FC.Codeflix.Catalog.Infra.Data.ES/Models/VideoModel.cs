using FC.Codeflix.Catalog.Domain.Enums;

namespace FC.Codeflix.Catalog.Infra.Data.ES.Models;

public class VideoModel
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
    public List<VideoCategoryModel> Categories { get; set; } = null!;
    public List<VideoGenreModel> Genres { get; set; } = null!;
    public List<VideoCastMemberModel> CastMembers { get; set; } = null!;
}

public class VideoGenreModel
{
    public VideoGenreModel(Guid id, string name)
    {
        Id = id;
        Name = name;
    }

    public Guid Id { get; set; }
    public string Name { get; set; }
}

public class VideoCategoryModel
{
    public VideoCategoryModel(Guid id, string name)
    {
        Id = id;
        Name = name;
    }

    public Guid Id { get; set; }
    public string Name { get; set; }
}

public class VideoCastMemberModel
{
    public VideoCastMemberModel(Guid id, string name, CastMemberType type)
    {
        Id = id;
        Name = name;
        Type = type;
    }

    public Guid Id { get; set; }
    public string Name { get; set; }
    public CastMemberType Type { get; set; }
}