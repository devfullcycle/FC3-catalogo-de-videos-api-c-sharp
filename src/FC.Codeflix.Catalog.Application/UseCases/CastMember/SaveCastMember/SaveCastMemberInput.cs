using FC.Codeflix.Catalog.Application.UseCases.CastMember.Common;
using FC.Codeflix.Catalog.Domain.Enums;
using MediatR;

namespace FC.Codeflix.Catalog.Application.UseCases.CastMember.SaveCastMember;

public class SaveCastMemberInput : IRequest<CastMemberModelOutput>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public CastMemberType Type { get; set; }
    public DateTime CreatedAt { get; set; }

    public SaveCastMemberInput(Guid id, string name, CastMemberType type, DateTime createdAt)
    {
        Id = id;
        Name = name;
        Type = type;
        CreatedAt = createdAt;
    }

    public Domain.Entity.CastMember ToCastMember()
        => new(Id, Name, Type, CreatedAt);
}