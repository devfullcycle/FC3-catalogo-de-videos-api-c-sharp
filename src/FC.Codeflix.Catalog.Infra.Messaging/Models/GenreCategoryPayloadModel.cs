using System.Text.Json.Serialization;

namespace FC.Codeflix.Catalog.Infra.Messaging.Models;

public class GenreCategoryPayloadModel : GenrePayloadModel
{
    [JsonPropertyName("GenreId")]
    public override Guid Id { get; set; }
}