using FC.Codeflix.Catalog.Domain.Entity;

namespace FC.Codeflix.Catalog.Infra.Data.ES.Models;
public class CategoryModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }

    public static CategoryModel FromEntity(Category entity)
        => new CategoryModel
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            IsActive = entity.IsActive,
            CreatedAt = entity.CreatedAt,
        };

    public Category ToEntity()
        => new(Id, Name, Description, CreatedAt, IsActive);
}
