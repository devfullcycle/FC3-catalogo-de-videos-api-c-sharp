using FC.Codeflix.Catalog.Domain.Enums;
using FC.Codeflix.Catalog.Domain.Validation;
using FC.Codeflix.Catalog.Domain.ValueObjects;

namespace FC.Codeflix.Catalog.Domain.Entity;

public class Video
{
    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public int YearLaunched { get; private set; }
    public int Duration { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public Rating Rating { get; private set; }
    public Medias Medias { get; private set; }
    
    private readonly List<Category> _categories = new();
    public IReadOnlyList<Category> Categories => _categories.AsReadOnly();
    
    private readonly List<Genre> _genres = new();
    public IReadOnlyList<Genre> Genres => _genres.AsReadOnly();
    
    private readonly List<CastMember> _castMembers = new();
    public IReadOnlyList<CastMember> CastMembers => _castMembers.AsReadOnly();

    public Video(
        Guid id,
        string title,
        string description,
        int yearLaunched,
        int duration,
        DateTime createdAt,
        Rating rating,
        Medias medias)
    {
        Id = id;
        Title = title;
        Description = description;
        YearLaunched = yearLaunched;
        Duration = duration;
        CreatedAt = createdAt;
        Rating = rating;
        Medias = medias;
        
        Validate();
    }
    
    public void AddCategories(params Category[] categories)
    {
        DomainValidation.NotNull(categories, nameof(categories));
        _categories.AddRange(categories);
    }
    
    public void AddGenres(params Genre[] genres)
    {
        DomainValidation.NotNull(genres, nameof(genres));
        _genres.AddRange(genres);
    }
    
    public void AddCastMembers(params CastMember[] castMembers)
    {
        DomainValidation.NotNull(castMembers, nameof(castMembers));
        _castMembers.AddRange(castMembers);
    }

    private void Validate()
    {
        DomainValidation.NotNullOrEmpty(Id, nameof(Id));
        DomainValidation.NotNullOrEmpty(Title, nameof(Title));
        DomainValidation.NotNullOrEmpty(Description, nameof(Description));
        DomainValidation.NotNegativeOrZero(YearLaunched, nameof(YearLaunched));
        DomainValidation.NotNegativeOrZero(Duration, nameof(Duration));
        DomainValidation.NotMinDateTime(CreatedAt, nameof(CreatedAt));
        DomainValidation.IsDefined(Rating, nameof(Rating));
        DomainValidation.NotNull(Medias, nameof(Medias));
    }
}