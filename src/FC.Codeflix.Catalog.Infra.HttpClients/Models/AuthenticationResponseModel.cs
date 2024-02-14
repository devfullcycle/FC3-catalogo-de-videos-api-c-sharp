using System.Text.Json.Serialization;

namespace FC.Codeflix.Catalog.Infra.HttpClients.Models;

public class AuthenticationResponseModel
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; } = null!;
    [JsonPropertyName("expires_in")]
    public int ExpiresInSeconds { get; set; }
}