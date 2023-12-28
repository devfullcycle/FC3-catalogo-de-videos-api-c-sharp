using FC.Codeflix.Catalog.Api.Common;
using FC.Codeflix.Catalog.Application.Common;
using FC.Codeflix.Catalog.Application.UseCases.Genre.Common;

namespace FC.Codeflix.Catalog.Api.Genres;

public class SearchGenrePayload : SearchPayload<GenrePayload>
{
    public static SearchGenrePayload FromSearchListOutput(
        SearchListOutput<GenreModelOutput> output)
        => new()
        {
            CurrentPage = output.CurrentPage,
            PerPage = output.PerPage,
            Total = output.Total,
            Items = output.Items.Select(GenrePayload.FromGenreModelOutput).ToList()
        };
}