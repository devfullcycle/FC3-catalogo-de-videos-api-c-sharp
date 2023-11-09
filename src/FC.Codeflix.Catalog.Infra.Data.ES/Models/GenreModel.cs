namespace FC.Codeflix.Catalog.Infra.Data.ES.Models;

public class GenreModel
{
    public Guid Id { get;  set; }
    public string Name { get; set; } = null!;
    public bool IsActive { get;  set; }
    public DateTime CreatedAt { get;  set; }

    public List<GenreCategoryModel> Categories { get; set; } = null!;
}

public class GenreCategoryModel
{
    public Guid Id { get;  set; }
    public string Name { get; set; } = null!; 
}