using System.Text.Json;
using FC.Codeflix.Catalog.Infra.HttpClients.Extensions;

namespace FC.Codeflix.Catalog.Infra.HttpClients.Configuration;

public class JsonSnakeCasePolicy: JsonNamingPolicy
{
    public override string ConvertName(string name)
        => name.ToSnakeCase();
}