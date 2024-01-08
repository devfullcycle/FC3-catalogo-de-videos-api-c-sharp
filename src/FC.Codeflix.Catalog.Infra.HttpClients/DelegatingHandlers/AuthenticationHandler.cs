using System.Net.Http.Headers;
using FC.Codeflix.Catalog.Infra.HttpClients.HttpClients;
using FC.Codeflix.Catalog.Infra.HttpClients.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace FC.Codeflix.Catalog.Infra.HttpClients.DelegatingHandlers;

internal class AuthenticationHandler : DelegatingHandler
{
    private readonly AuthenticationClient _authClient;
    private readonly CredentialsModel _credentials;
    private readonly IMemoryCache _cache;
    private const string CacheKey = "token";
    public AuthenticationHandler(
        AuthenticationClient authClient,
        IOptions<CredentialsModel> credentials,
        IMemoryCache cache)
    {
        _authClient = authClient;
        _cache = cache;
        _credentials = credentials.Value;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (!_cache.TryGetValue(CacheKey, out string? token))
        {
            var authResponse = await _authClient.GetAccessToken(_credentials.Username, _credentials.Password, cancellationToken);
            token = authResponse.AccessToken;
            var cacheOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromSeconds(authResponse.ExpiresInSeconds - 5));
            _cache.Set(CacheKey, token, cacheOptions);
        }
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return await base.SendAsync(request, cancellationToken);
    }
}