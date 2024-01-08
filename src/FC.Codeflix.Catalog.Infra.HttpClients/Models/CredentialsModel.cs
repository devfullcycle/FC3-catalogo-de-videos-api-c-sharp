namespace FC.Codeflix.Catalog.Infra.HttpClients.Models;

public class CredentialsModel
{
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string ClientId { get; set; } = null!;
    public string ClientSecret { get; set; } = null!;
}