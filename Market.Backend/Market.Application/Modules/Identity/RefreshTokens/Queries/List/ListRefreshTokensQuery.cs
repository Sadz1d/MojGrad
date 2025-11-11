using Market.Application.Modules.Identity.RefreshTokens.Queries.List;
using Market.Domain.Common;
using MediatR;

namespace Market.Application.Modules.Identity.RefreshTokens.Queries.List;

public sealed class ListRefreshTokensQuery : BasePagedQuery<ListRefreshTokensQueryDto>, IRequest<PageResult<ListRefreshTokensQueryDto>>
{
    public int? UserId { get; init; } // opcionalno filtriranje po korisniku
    public bool? OnlyActive { get; init; } // true = samo ne-revoked
}
