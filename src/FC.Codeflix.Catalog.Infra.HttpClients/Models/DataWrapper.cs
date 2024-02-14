namespace FC.Codeflix.Catalog.Infra.HttpClients.Models;

public class DataWrapper<T> where T : class
{
    public DataWrapper(T data)
    {
        Data = data;
    }

    public T Data { get; set; }
    
}