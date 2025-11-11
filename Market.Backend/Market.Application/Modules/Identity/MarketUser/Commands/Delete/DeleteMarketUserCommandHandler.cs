using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Domain.Entities.Identity;
using Market.Application.Common.Exceptions;

namespace Market.Application.Modules.Identity.Users.Commands.Delete;

public sealed class DeleteMarketUserCommandHandler : IRequestHandler<DeleteMarketUserCommand, Unit>
{
    private readonly IAppDbContext _ctx;

    public DeleteMarketUserCommandHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<Unit> Handle(DeleteMarketUserCommand request, CancellationToken ct)
    {
        var user = await _ctx.Users
            .FirstOrDefaultAsync(u => u.Id == request.Id, ct);

        if (user == null)
            throw new MarketNotFoundException($"User with Id {request.Id} not found.");

        _ctx.Users.Remove(user);
        await _ctx.SaveChangesAsync(ct);

        return Unit.Value;
    }
}
