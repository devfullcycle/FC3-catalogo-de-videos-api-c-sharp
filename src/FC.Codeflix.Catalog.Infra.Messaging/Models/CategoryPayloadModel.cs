using FC.Codeflix.Catalog.Application.UseCases.Category.DeleteCategory;
using FC.Codeflix.Catalog.Application.UseCases.Category.SaveCategory;

namespace FC.Codeflix.Catalog.Infra.Messaging.Models;

public class CategoryPayloadModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public SaveCategoryInput ToSaveCategoryInput()
        => new(Id, Name, Description, CreatedAt, IsActive);
    
    public DeleteCategoryInput ToDeleteCategoryInput()
        => new(Id);
}