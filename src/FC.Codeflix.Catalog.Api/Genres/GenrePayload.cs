using FC.Codeflix.Catalog.Application.UseCases.Genre.Common;
using FC.Codeflix.Catalog.Domain.Entity;

namespace FC.Codeflix.Catalog.Api.Genres;

public class GenrePayload
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public IEnumerable<GenreCategoryPayload>? Categories { get; set; }

    public static GenrePayload FromGenreModelOutput(GenreModelOutput output)
        => new()
        {
            Id = output.Id,
            Name = output.Name,
            IsActive = output.IsActive,
            CreatedAt = output.CreatedAt,
            Categories = output.Categories.Select(category => new GenreCategoryPayload(category.Id, category.Name))
        };
}

public class GenreCategoryPayload
{
    public Guid Id { get; set; }
    public string? Name { get; set; }

    public GenreCategoryPayload(Guid id, string? name)
    {
        Id = id;
        Name = name;
    }
}