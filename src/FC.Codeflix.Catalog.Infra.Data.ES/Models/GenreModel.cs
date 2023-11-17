using FC.Codeflix.Catalog.Domain.Entity;

namespace FC.Codeflix.Catalog.Infra.Data.ES.Models;

public class GenreModel
{
    public Guid Id { get;  set; }
    public string Name { get; set; } = null!;
    public bool IsActive { get;  set; }
    public DateTime CreatedAt { get;  set; }

    public List<GenreCategoryModel> Categories { get; set; } = null!;

    public static GenreModel FromEntity(Genre genre)
        => new()
        {
            Id = genre.Id,
            Name = genre.Name,
            IsActive = genre.IsActive,
            CreatedAt = genre.CreatedAt,
            Categories = genre.Categories.Select(c => new GenreCategoryModel
            {
                Id = c.Id,
                Name = c.Name
            }).ToList()
        };

    public Genre ToEntity()
        => new(
            Id,
            Name,
            IsActive,
            CreatedAt,
            Categories.Select(item => new Category(item.Id, item.Name)));
}

public class GenreCategoryModel
{
    public Guid Id { get;  set; }
    public string Name { get; set; } = null!; 
}