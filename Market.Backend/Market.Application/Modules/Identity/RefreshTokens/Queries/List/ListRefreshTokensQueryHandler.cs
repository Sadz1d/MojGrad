using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Domain.Entities.Identity;

namespace Market.Application.Modules.Identity.RefreshTokens.Queries.List;

public sealed class ListRefreshTokensQueryHandler
    : IRequestHandler<ListRefreshTokensQuery, PageResult<ListRefreshTokensQueryDto>>
{
    private readonly IAppDbContext _ctx;

    public ListRefreshTokensQueryHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<PageResult<ListRefreshTokensQueryDto>> Handle(
        ListRefreshTokensQuery request, CancellationToken ct)
    {
        IQueryable<RefreshTokenEntity> query = _ctx.RefreshTokens.AsNoTracking();

        if (request.UserId.HasValue)
            query = query.Where(rt => rt.UserId == request.UserId.Value);

        if (request.OnlyActive.HasValue && request.OnlyActive.Value)
            query = query.Where(rt => !rt.IsRevoked);

        var projected = query
            .OrderByDescending(rt => rt.ExpiresAtUtc)
            .Select(rt => new ListRefreshTokensQueryDto
            {
                Id = rt.Id,
                TokenHash = rt.TokenHash,
                UserId = rt.UserId,
                IsRevoked = rt.IsRevoked,
                ExpiresAtUtc = rt.ExpiresAtUtc,
                Fingerprint = rt.Fingerprint,
                RevokedAtUtc = rt.RevokedAtUtc
            });

        return await PageResult<ListRefreshTokensQueryDto>
            .FromQueryableAsync(projected, request.Paging, ct);
    }
}
