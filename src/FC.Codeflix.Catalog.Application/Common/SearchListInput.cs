using FC.Codeflix.Catalog.Domain.Repositories.DTOs;

namespace FC.Codeflix.Catalog.Application.Common;
public abstract class SearchListInput
{
    public int Page { get; set; }
    public int PerPage { get; set; }
    public string Search { get; set; }
    public string OrderBy { get; set; }
    public SearchOrder Order { get; set; }

    public SearchListInput(
        int page,
        int perPage,
        string search,
        string orderBy,
        SearchOrder order)
    {
        Page = page;
        PerPage = perPage;
        Search = search;
        OrderBy = orderBy;
        Order = order;
    }

    public SearchInput ToSearchInput()
        => new(Page, PerPage, Search, OrderBy, Order);
}
