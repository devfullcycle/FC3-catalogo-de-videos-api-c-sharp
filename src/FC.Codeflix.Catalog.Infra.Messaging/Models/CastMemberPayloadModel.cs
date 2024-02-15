using FC.Codeflix.Catalog.Domain.Enums;

namespace FC.Codeflix.Catalog.Infra.Messaging.Models;

public class CastMemberPayloadModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public CastMemberType Type { get; set; }
    public DateTime CreatedAt { get; set; }
    

}