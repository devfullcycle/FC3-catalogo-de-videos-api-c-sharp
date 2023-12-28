using FC.Codeflix.Catalog.Application.UseCases.Genre.Common;
using MediatR;

namespace FC.Codeflix.Catalog.Application.UseCases.Genre.GetGenresByIds;

public class GetGenresByIdsInput
    : IRequest<IEnumerable<GenreModelOutput>>
{
    public GetGenresByIdsInput(IEnumerable<Guid> ids)
    {
        Ids = ids;
    }

    public IEnumerable<Guid> Ids { get; }
}