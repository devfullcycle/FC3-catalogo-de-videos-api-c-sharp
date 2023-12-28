using FC.Codeflix.Catalog.Application.UseCases.Genre.Common;
using MediatR;

namespace FC.Codeflix.Catalog.Application.UseCases.Genre.GetGenresByIds;

public interface IGetGenresByIds
    : IRequestHandler<GetGenresByIdsInput, IEnumerable<GenreModelOutput>>
{
    
}