using MediatR;

namespace FC.Codeflix.Catalog.Application.UseCases.CastMember.DeleteCastMember;

public class DeleteCastMemberInput : IRequest
{
    public DeleteCastMemberInput(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; private set; }
}