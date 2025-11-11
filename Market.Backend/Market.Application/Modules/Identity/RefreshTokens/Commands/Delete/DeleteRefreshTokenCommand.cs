using MediatR;

namespace Market.Application.Modules.Identity.RefreshTokens.Commands.Delete;

public sealed class DeleteRefreshTokenCommand : IRequest<Unit>
{
    public required int Id { get; init; }
}
