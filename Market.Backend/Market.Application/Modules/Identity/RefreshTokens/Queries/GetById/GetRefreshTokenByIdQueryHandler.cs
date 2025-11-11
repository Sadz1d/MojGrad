using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Domain.Entities.Identity;
using Market.Application.Common.Exceptions;

namespace Market.Application.Modules.Identity.RefreshTokens.Queries.GetById;

public sealed class GetRefreshTokenByIdQueryHandler
    : IRequestHandler<GetRefreshTokenByIdQuery, GetRefreshTokenByIdQueryDto>
{
    private readonly IAppDbContext _ctx;

    public GetRefreshTokenByIdQueryHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<GetRefreshTokenByIdQueryDto> Handle(GetRefreshTokenByIdQuery request, CancellationToken ct)
    {
        var entity = await _ctx.RefreshTokens
            .AsNoTracking()
            .FirstOrDefaultAsync(rt => rt.Id == request.Id, ct);

        if (entity == null)
            throw new MarketNotFoundException($"Refresh token with Id {request.Id} not found.");

        return new GetRefreshTokenByIdQueryDto
        {
            Id = entity.Id,
            TokenHash = entity.TokenHash,
            UserId = entity.UserId,
            IsRevoked = entity.IsRevoked,
            ExpiresAtUtc = entity.ExpiresAtUtc,
            Fingerprint = entity.Fingerprint,
            RevokedAtUtc = entity.RevokedAtUtc
        };
    }
}
