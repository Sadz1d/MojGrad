using MediatR;

namespace Market.Application.Modules.Identity.RefreshTokens.Commands.Create;

public sealed class CreateRefreshTokenCommand : IRequest<int>
{
    public required int UserId { get; init; }
    public required string TokenHash { get; init; }
    public required DateTime ExpiresAtUtc { get; init; }
    public string? Fingerprint { get; init; }
}
