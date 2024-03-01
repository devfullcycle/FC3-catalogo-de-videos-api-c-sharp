using FC.Codeflix.Catalog.Application.Common;
using FC.Codeflix.Catalog.Application.UseCases.Video.Common;
using FC.Codeflix.Catalog.Domain.Repositories.DTOs;
using MediatR;

namespace FC.Codeflix.Catalog.Application.UseCases.Video.SearchVideo;

public class SearchVideoInput : 
    SearchListInput, IRequest<SearchListOutput<VideoModelOutput>>
{
    public SearchVideoInput(
        int page = 1,
        int perPage = 20,
        string search = "",
        string orderBy = "",
        SearchOrder order = SearchOrder.Asc) : base(page, perPage, search, orderBy, order)
    {
    }
}