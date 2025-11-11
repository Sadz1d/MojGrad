using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Application.Common.Exceptions;
using Market.Domain.Entities.Identity;

namespace Market.Application.Modules.Identity.RefreshTokens.Commands.Delete;

public sealed class DeleteRefreshTokenCommandHandler
    : IRequestHandler<DeleteRefreshTokenCommand, Unit>
{
    private readonly IAppDbContext _ctx;

    public DeleteRefreshTokenCommandHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<Unit> Handle(DeleteRefreshTokenCommand request, CancellationToken ct)
    {
        var token = await _ctx.RefreshTokens
            .FirstOrDefaultAsync(x => x.Id == request.Id, ct);

        if (token == null)
            throw new MarketNotFoundException($"Refresh token with Id {request.Id} not found.");

        _ctx.RefreshTokens.Remove(token);
        await _ctx.SaveChangesAsync(ct);

        return Unit.Value;
    }
}
