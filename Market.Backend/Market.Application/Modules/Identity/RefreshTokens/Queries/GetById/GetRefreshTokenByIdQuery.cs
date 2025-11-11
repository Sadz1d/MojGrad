using MediatR;

namespace Market.Application.Modules.Identity.RefreshTokens.Queries.GetById;

public sealed class GetRefreshTokenByIdQuery : IRequest<GetRefreshTokenByIdQueryDto>
{
    public int Id { get; init; }
}
