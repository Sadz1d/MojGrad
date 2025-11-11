using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Application.Common.Exceptions;
using Market.Domain.Entities.Identity;

namespace Market.Application.Modules.Identity.Profiles.Commands.Delete;

public sealed class DeleteProfileCommandHandler : IRequestHandler<DeleteProfileCommand, Unit>
{
    private readonly IAppDbContext _ctx;

    public DeleteProfileCommandHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<Unit> Handle(DeleteProfileCommand request, CancellationToken ct)
    {
        var profile = await _ctx.Profiles
            .FirstOrDefaultAsync(p => p.Id == request.Id, ct);

        if (profile == null)
            throw new MarketNotFoundException($"Profile with Id {request.Id} not found.");

        _ctx.Profiles.Remove(profile);
        await _ctx.SaveChangesAsync(ct);

        return Unit.Value;
    }
}
