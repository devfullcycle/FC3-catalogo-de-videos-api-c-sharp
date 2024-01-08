using FC.Codeflix.Catalog.Domain.Entity;

namespace FC.Codeflix.Catalog.Infra.HttpClients.Models;

public class GenreOutputModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public IEnumerable<GenreCategoryOutputModel> Categories { get; set; } = null!;

    public Genre ToGenre()
        => new Genre(
            Id,
            Name,
            IsActive,
            CreatedAt,
            Categories.Select(c => new Category(c.Id, c.Name)));
}

public class GenreCategoryOutputModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
} 