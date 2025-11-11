using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Application.Common.Exceptions;
using Market.Domain.Entities.Identity;

namespace Market.Application.Modules.Identity.RefreshTokens.Commands.Update;

public sealed class UpdateRefreshTokenCommandHandler
    : IRequestHandler<UpdateRefreshTokenCommand, Unit>
{
    private readonly IAppDbContext _ctx;

    public UpdateRefreshTokenCommandHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<Unit> Handle(UpdateRefreshTokenCommand request, CancellationToken ct)
    {
        var token = await _ctx.RefreshTokens
            .FirstOrDefaultAsync(x => x.Id == request.Id, ct);

        if (token == null)
            throw new MarketNotFoundException($"Refresh token with Id {request.Id} not found.");

        if (request.IsRevoked.HasValue)
            token.IsRevoked = request.IsRevoked.Value;

        if (request.ExpiresAtUtc.HasValue)
            token.ExpiresAtUtc = request.ExpiresAtUtc.Value;

        if (request.Fingerprint != null)
            token.Fingerprint = request.Fingerprint.Trim();

        await _ctx.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
