using System.Net.Http.Json;
using FC.Codeflix.Catalog.Infra.HttpClients.Models;
using Microsoft.Extensions.Options;

namespace FC.Codeflix.Catalog.Infra.HttpClients.HttpClients;

internal class AuthenticationClient
{
    private readonly HttpClient _client;
    private readonly CredentialsModel _credentials;

    public AuthenticationClient(
        HttpClient client,
        IOptions<CredentialsModel> credentials)
    {
        _client = client;
        _credentials = credentials.Value;
    }

    public async Task<string> GetAccessToken(
        string username,
        string password,
        CancellationToken cancellationToken = default)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "/realms/fc3-codeflix/protocol/openid-connect/token");
        var collection = new List<KeyValuePair<string, string>>();
        collection.Add(new("grant_type", "password"));
        collection.Add(new("client_id", _credentials.ClientId));
        collection.Add(new("client_secret", _credentials.ClientSecret));
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