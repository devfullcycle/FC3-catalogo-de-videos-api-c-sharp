using FC.Codeflix.Catalog.Application.UseCases.CastMember.Common;
using MediatR;

namespace FC.Codeflix.Catalog.Application.UseCases.CastMember.SaveCastMember;

public interface ISaveCastMember : IRequestHandler<SaveCastMemberInput, CastMemberModelOutput>
{
    
}