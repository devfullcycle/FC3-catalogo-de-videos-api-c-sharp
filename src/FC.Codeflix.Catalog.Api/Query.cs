namespace FC.Codeflix.Catalog.Api;

public class Query
{
    public string Hello(string name = "World")
        => $"Hello, {name}!";
}
