using System.Net.Http.Headers;
using FC.Codeflix.Catalog.Infra.HttpClients.HttpClients;
using FC.Codeflix.Catalog.Infra.HttpClients.Models;
using Microsoft.Extensions.Options;

namespace FC.Codeflix.Catalog.Infra.HttpClients.DelegatingHandlers;

internal class AuthenticationHandler : DelegatingHandler
{
    private readonly AuthenticationClient _authClient;
    private readonly CredentialsModel _credentials;

    public AuthenticationHandler(
        AuthenticationClient authClient,
        IOptions<CredentialsModel> credentials)
    {
        _authClient = authClient;
        _credentials = credentials.Value;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = await _authClient.GetAccessToken(_credentials.Username, _credentials.Password, cancellationToken);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return await base.SendAsync(request, cancellationToken);
    }
}