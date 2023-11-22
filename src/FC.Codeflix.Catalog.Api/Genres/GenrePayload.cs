namespace FC.Codeflix.Catalog.Api.Genres;

public class GenrePayload
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public IEnumerable<GenreCategoryPayload>? Categories { get; set; }
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