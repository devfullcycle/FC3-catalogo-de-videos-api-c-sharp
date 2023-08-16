namespace FC.Codeflix.Catalog.Domain.Repositories.DTOs;
public class SearchOutput<T>
    where T : class
{
    public int CurrentPage { get; set; }
    public int PerPage { get; set; }
    public int Total { get; set; }
    public IReadOnlyList<T> Items { get; set; }
    public SearchOutput(
        int currentPage,
        int perPage,
        int total,
        IReadOnlyList<T> items)
    {
        CurrentPage = currentPage;
        PerPage = perPage;
        Total = total;
        Items = items;
    }
}
