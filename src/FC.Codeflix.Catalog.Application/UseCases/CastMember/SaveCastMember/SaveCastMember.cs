using FC.Codeflix.Catalog.Application.UseCases.CastMember.Common;
using FC.Codeflix.Catalog.Domain.Repositories;

namespace FC.Codeflix.Catalog.Application.UseCases.CastMember.SaveCastMember;

public class SaveCastMember : ISaveCastMember
{
    private readonly ICastMemberRepository _repository;

    public SaveCastMember(ICastMemberRepository repository)
    {
        _repository = repository;
    }

    public async Task<CastMemberModelOutput> Handle(SaveCastMemberInput request, CancellationToken cancellationToken)
    {
        var castMember = request.ToCastMember();
        await _repository.SaveAsync(castMember, cancellationToken);
        return CastMemberModelOutput.FromCastMember(castMember);
    }
}