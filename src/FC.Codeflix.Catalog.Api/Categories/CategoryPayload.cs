using FC.Codeflix.Catalog.Application.UseCases.Category.Common;

namespace FC.Codeflix.Catalog.Api.Categories;

public class CategoryPayload
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }

    public static CategoryPayload FromCategoryModelOutput(CategoryModelOutput output)
        => new CategoryPayload
        {
            CreatedAt = output.CreatedAt,
            Name = output.Name,
            Description = output.Description,
            IsActive = output.IsActive,
            Id = output.Id,
        };
}
