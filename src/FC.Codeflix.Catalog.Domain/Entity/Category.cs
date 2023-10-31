using FC.Codeflix.Catalog.Domain.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FC.Codeflix.Catalog.Domain.Entity;
public class Category
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public Category(
        Guid id,
        string? name)
            : this(id, name, null, DateTime.Now, true)
    { }
    
    public Category(
        Guid id,
        string? name,
        string? description,
        DateTime createdAt,
        bool isActive = true)
    {
        Id = id;
        Name = name!;
        Description = description!;
        IsActive = isActive;
        CreatedAt = createdAt;

        Validate();
    }

    private void Validate()
    {
        DomainValidation.NotNullOrEmpty(Id, nameof(Id));
        DomainValidation.NotNullOrEmpty(Name, nameof(Name));
    }
}
