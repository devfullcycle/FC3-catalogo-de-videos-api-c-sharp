using FC.Codeflix.Catalog.Domain.Entity;
using FC.Codeflix.Catalog.Domain.Enums;
using FC.Codeflix.Catalog.Domain.ValueObjects;

namespace FC.Codeflix.Catalog.Infra.HttpClients.Models;

public class VideoOutputModel
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Rating { get; set; } = null!;
    public int YearLaunched { get; set; }
    public int Duration { get; set; }
    public List<VideoModelOutputRelation> Categories { get; set; } = null!;
    public List<VideoModelOutputRelation> Genres { get; set; } = null!;
    public List<VideoModelOutputRelation> CastMembers { get; set; } = null!;
    public string? ThumbFileUrl { get; set; }
    public string? BannerFileUrl { get; set; }
    public string? ThumbHalfFileUrl { get; set; }
    public string? VideoFileUrl { get; set; }
    public string? TrailerFileUrl { get; set; }
    
    public Video ToVideo(CastMember[] castMembers)
    {
        var video = new Video(
            Id,
            Title,
            Description,
            YearLaunched,
            Duration,
            CreatedAt,
            (Rating)Enum.Parse(typeof(Rating), Rating, true),
            new Medias(
                ThumbFileUrl,
                ThumbHalfFileUrl,
                BannerFileUrl,
                VideoFileUrl,
                TrailerFileUrl
            ));
        video.AddCategories(Categories.Select(relation => relation.ToCategory()).ToArray());
        video.AddGenres(Genres.Select(relation => relation.ToGenre()).ToArray());
        video.AddCastMembers(castMembers);
        return video;
    }
    
}

public class VideoModelOutputRelation
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    
    public Category ToCategory() => new(Id, Name);

    public Genre ToGenre() => new(Id, Name);
}