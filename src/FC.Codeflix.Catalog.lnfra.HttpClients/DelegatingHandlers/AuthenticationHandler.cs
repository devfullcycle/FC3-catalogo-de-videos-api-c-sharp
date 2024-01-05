using System.Net.Http.Headers;
using FC.Codeflix.Catalog.lnfra.HttpClients.HttpClients;

namespace FC.Codeflix.Catalog.lnfra.HttpClients.DelegatingHandlers;

internal class AuthenticationHandler : DelegatingHandler
{
    private readonly AuthenticationClient _authClient;

    public AuthenticationHandler(AuthenticationClient authClient)
    {
        _authClient = authClient;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = await _authClient.GetAccessToken("admin", "123456", cancellationToken);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return await base.SendAsync(request, cancellationToken);
    }
}