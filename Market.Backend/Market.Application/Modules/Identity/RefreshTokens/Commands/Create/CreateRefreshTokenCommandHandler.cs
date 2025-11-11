using MediatR;
using Market.Domain.Entities.Identity;

namespace Market.Application.Modules.Identity.RefreshTokens.Commands.Create;

public sealed class CreateRefreshTokenCommandHandler
    : IRequestHandler<CreateRefreshTokenCommand, int>
{
    private readonly IAppDbContext _ctx;

    public CreateRefreshTokenCommandHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<int> Handle(CreateRefreshTokenCommand request, CancellationToken ct)
    {
        var entity = new RefreshTokenEntity
        {
            UserId = request.UserId,
            TokenHash = request.TokenHash,
            ExpiresAtUtc = request.ExpiresAtUtc,
            Fingerprint = request.Fingerprint,
            IsRevoked = false
        };

        _ctx.RefreshTokens.Add(entity);
        await _ctx.SaveChangesAsync(ct);

        return entity.Id;
    }
}
