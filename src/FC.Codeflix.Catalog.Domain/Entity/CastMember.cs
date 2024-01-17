using FC.Codeflix.Catalog.Domain.Enums;
using FC.Codeflix.Catalog.Domain.Validation;

namespace FC.Codeflix.Catalog.Domain.Entity;

public class CastMember
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public CastMemberType Type { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public CastMember(Guid id, string name, CastMemberType type)
        : this(id, name, type, DateTime.Now)
    {
    }
    
    public CastMember(Guid id, string name, CastMemberType type, DateTime createdAt)
    {
        Id = id;
        Name = name;
        Type = type;
        CreatedAt = createdAt;

        Validate();
    }

    private void Validate()
    {
        DomainValidation.NotNullOrEmpty(Id, nameof(Id));
        DomainValidation.NotNullOrEmpty(Name, nameof(Name));
        DomainValidation.IsDefined(Type, nameof(Type));
    }
}