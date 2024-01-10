using System.Text.Json;

namespace FC.Codeflix.Catalog.Infra.HttpClients.Configuration;

public class SerializerConfiguration
{
    public static readonly JsonSerializerOptions SnakeCaseSerializerOptions = new(JsonSerializerDefaults.Web)
    {
        PropertyNamingPolicy = new JsonSnakeCasePolicy()
    };
}