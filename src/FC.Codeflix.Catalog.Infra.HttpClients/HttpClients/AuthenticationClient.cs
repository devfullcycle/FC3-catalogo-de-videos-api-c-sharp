using System.Net.Http.Json;
using FC.Codeflix.Catalog.Infra.HttpClients.Models;

namespace FC.Codeflix.Catalog.Infra.HttpClients.HttpClients;

internal class AuthenticationClient
{
    private readonly HttpClient _client;

    public AuthenticationClient(HttpClient client)
    {
        _client = client;
    }

    public async Task<string> GetAccessToken(
        string username,
        string password,
        CancellationToken cancellationToken = default)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "/realms/fc3-codeflix/protocol/openid-connect/token");
        var collection = new List<KeyValuePair<string, string>>();
        collection.Add(new("grant_type", "password"));
        collection.Add(new("client_id", "fc3-catalog-admin"));
        collection.Add(new("client_secret", "b6zkvQteR98PR5iGwf5Z2ClFqNOipKdx"));
        collection.Add(new("username", username));
        collection.Add(new("password", password));
        var content = new FormUrlEncodedContent(collection);
        request.Content = content;
        var response = await _client.SendAsync(request, cancellationToken);
        var responseBody =
            await response.Content.ReadFromJsonAsync<AuthenticationResponseModel>(cancellationToken: cancellationToken);
        return responseBody!.AccessToken;
    }
}