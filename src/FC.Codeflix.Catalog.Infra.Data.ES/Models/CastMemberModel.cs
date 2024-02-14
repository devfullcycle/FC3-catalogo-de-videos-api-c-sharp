using FC.Codeflix.Catalog.Domain.Entity;
using FC.Codeflix.Catalog.Domain.Enums;

namespace FC.Codeflix.Catalog.Infra.Data.ES.Models;

public class CastMemberModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public CastMemberType Type { get; set; }
    public DateTime CreatedAt { get; set; }

    public CastMember ToEntity()
        => new(Id, Name, Type, CreatedAt);

    public static CastMemberModel FromEntity(CastMember castMember)
        => new()
        {
            Id = castMember.Id,
            Name = castMember.Name,
            Type = castMember.Type,
            CreatedAt = castMember.CreatedAt
        };
}