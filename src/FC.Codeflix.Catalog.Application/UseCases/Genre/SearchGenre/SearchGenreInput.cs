using FC.Codeflix.Catalog.Application.Common;
using FC.Codeflix.Catalog.Application.UseCases.Genre.Common;
using FC.Codeflix.Catalog.Domain.Repositories.DTOs;
using MediatR;

namespace FC.Codeflix.Catalog.Application.UseCases.Genre.SearchGenre;

public class SearchGenreInput
    : SearchListInput, IRequest<SearchListOutput<GenreModelOutput>>
{
    public SearchGenreInput(
        int page = 1,
        int perPage = 20,
        string search = "",
        string orderBy = "",
        SearchOrder order = SearchOrder.Asc) : base(page, perPage, search, orderBy, order)
    {
    }
}