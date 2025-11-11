using MediatR;
using System.Text.Json.Serialization;

namespace Market.Application.Modules.Identity.RefreshTokens.Commands.Update;

public sealed class UpdateRefreshTokenCommand : IRequest<Unit>
{
    [JsonIgnore]
    public int Id { get; set; } // dolazi iz rute

    public bool? IsRevoked { get; set; }
    public DateTime? ExpiresAtUtc { get; set; }
    public string? Fingerprint { get; set; }
}
