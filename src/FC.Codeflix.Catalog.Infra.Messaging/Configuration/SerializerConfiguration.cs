using System.Text.Json;
using FC.Codeflix.Catalog.Infra.Messaging.JsonConverters;

namespace FC.Codeflix.Catalog.Infra.Messaging.Configuration;

public static class SerializerConfiguration
{
    public static readonly JsonSerializerOptions JsonSerializerOptions = new(JsonSerializerDefaults.Web)
    {
        Converters = { new BoolConverter(), new DateTimeConverter() }
    };
}