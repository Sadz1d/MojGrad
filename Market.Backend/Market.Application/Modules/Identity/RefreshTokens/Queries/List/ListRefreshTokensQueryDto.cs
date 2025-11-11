namespace Market.Application.Modules.Identity.RefreshTokens.Queries.List;

public sealed class ListRefreshTokensQueryDto
{
    public required int Id { get; init; }
    public required string TokenHash { get; init; }
    public required int UserId { get; init; }
    public required bool IsRevoked { get; init; }
    public DateTime ExpiresAtUtc { get; init; }
    public string? Fingerprint { get; init; }
    public DateTime? RevokedAtUtc { get; init; }
}
