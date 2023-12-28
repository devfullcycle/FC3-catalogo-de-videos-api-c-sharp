using FC.Codeflix.Catalog.Application.UseCases.Genre.Common;
using FC.Codeflix.Catalog.Domain.Repositories;

namespace FC.Codeflix.Catalog.Application.UseCases.Genre.GetGenresByIds;

public class GetGenresByIds
    : IGetGenresByIds
{
    private readonly IGenreRepository _repository;

    public GetGenresByIds(IGenreRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<GenreModelOutput>> Handle(GetGenresByIdsInput request, CancellationToken cancellationToken)
    {
        var genres = await _repository.GetGenresByIdsAsync(request.Ids, cancellationToken);
        return genres.Select(GenreModelOutput.FromGenre).ToList();
    }
}