using FC.Codeflix.Catalog.Application.UseCases.CastMember.Common;
using FC.Codeflix.Catalog.Domain.Enums;

namespace FC.Codeflix.Catalog.Api.CastMembers;

public class CastMemberPayload
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;
    public CastMemberType Type { get; private set; }
    public DateTime CreatedAt { get; private set; }
    
    public static CastMemberPayload FromCastMemberModelOutput(
        CastMemberModelOutput castMember)
        => new()
        {
            Id = castMember.Id,
            Name = castMember.Name,
            Type = castMember.Type,
            CreatedAt = castMember.CreatedAt
        };
}