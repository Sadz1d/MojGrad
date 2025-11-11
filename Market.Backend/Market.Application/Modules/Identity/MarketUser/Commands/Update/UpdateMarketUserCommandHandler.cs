using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Domain.Entities.Identity;
using Market.Application.Common.Exceptions;

namespace Market.Application.Modules.Identity.Users.Commands.Update;

public sealed class UpdateMarketUserCommandHandler : IRequestHandler<UpdateMarketUserCommand, Unit>
{
    private readonly IAppDbContext _ctx;

    public UpdateMarketUserCommandHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<Unit> Handle(UpdateMarketUserCommand request, CancellationToken ct)
    {
        var user = await _ctx.Users
            .FirstOrDefaultAsync(u => u.Id == request.Id, ct);

        if (user == null)
            throw new MarketNotFoundException($"User with Id {request.Id} not found.");

        // Update samo ako su vrijednosti poslane
        if (request.FirstName != null) user.FirstName = request.FirstName;
        if (request.LastName != null) user.LastName = request.LastName;
        if (request.Email != null) user.Email = request.Email;
        if (request.IsAdmin.HasValue) user.IsAdmin = request.IsAdmin.Value;
        if (request.IsManager.HasValue) user.IsManager = request.IsManager.Value;
        if (request.IsEmployee.HasValue) user.IsEmployee = request.IsEmployee.Value;
        if (request.IsEnabled.HasValue) user.IsEnabled = request.IsEnabled.Value;

        await _ctx.SaveChangesAsync(ct);

        return Unit.Value;
    }
}
