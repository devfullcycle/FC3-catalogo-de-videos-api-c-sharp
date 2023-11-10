using DomainEntity = FC.Codeflix.Catalog.Domain.Entity;

namespace FC.Codeflix.Catalog.Application.UseCases.Genre.Common;

public class GenreModelOutput
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public IReadOnlyList<GenreModelOutputCategory> Categories { get; private set; }
    
    public GenreModelOutput(
        Guid id,
        string? name,
        bool isActive,
        DateTime createdAt,
        IEnumerable<GenreModelOutputCategory> categories)
    {
        Id = id;
        Name = name!;
        IsActive = isActive;
        CreatedAt = createdAt;
        Categories = categories.ToList().AsReadOnly();
    }
    
    public static GenreModelOutput FromGenre(
        DomainEntity.Genre genre)
        => new(
            genre.Id,
            genre.Name,
            genre.IsActive,
            genre.CreatedAt,
            genre.Categories
                .Select(category => new GenreModelOutputCategory(category.Id, category.Name)));
}

public class GenreModelOutputCategory
{
    public Guid Id { get; set; }
    public string? Name { get; set; }

    public GenreModelOutputCategory(Guid id, string? name = null)
        => (Id, Name) = (id, name);
}