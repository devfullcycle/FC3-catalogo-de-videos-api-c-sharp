using FC.Codeflix.Catalog.Domain.Repositories.DTOs;

namespace FC.Codeflix.Catalog.Api.Genres;

[ExtendObjectType(OperationTypeNames.Query)]
public class GenreQueries
{
    public async Task<SearchGenrePayload> GetGenresAsync(
        int page = 1,
        int perPage = 10,
        string search = "",
        string sort = "",
        SearchOrder direction = SearchOrder.Asc,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    } 
}