namespace FC.Codeflix.Catalog.Api.Common;

public class SearchPayload<TPayload>
    where TPayload : class
{
    public int CurrentPage { get; set; }
    public int PerPage { get; set; }
    public int Total { get; set; }
    public IReadOnlyList<TPayload> Items { get; set; } = null!;
}
