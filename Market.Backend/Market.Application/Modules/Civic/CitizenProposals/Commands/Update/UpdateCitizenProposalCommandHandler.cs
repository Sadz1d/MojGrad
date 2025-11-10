using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Application.Common.Exceptions;
using Market.Domain.Entities.Civic;

namespace Market.Application.Modules.Civic.CitizenProposals.Commands.Update;

public sealed class UpdateCitizenProposalCommandHandler
    : IRequestHandler<UpdateCitizenProposalCommand, Unit>
{
    private readonly IAppDbContext _ctx;
    public UpdateCitizenProposalCommandHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<Unit> Handle(UpdateCitizenProposalCommand request, CancellationToken ct)
    {
        var entity = await _ctx.CitizenProposals
            .FirstOrDefaultAsync(p => p.Id == request.Id, ct);

        if (entity is null)
            throw new MarketNotFoundException($"CitizenProposal (Id={request.Id}) not found.");

        var normalizedTitle = request.Title.Trim();

        // zabrani duplikat naslova za istog korisnika (osim ove iste stavke)
        var exists = await _ctx.CitizenProposals
            .AnyAsync(p => p.Id != request.Id
                        && p.UserId == entity.UserId
                        && p.Title == normalizedTitle, ct);
        if (exists)
            throw new MarketConflictException("A proposal with the same title already exists for this user.");

        // ažuriranje polja
        entity.Title = normalizedTitle;
        entity.Text = request.Text.Trim();
        if (request.IsEnabled.HasValue)
            entity.IsEnabled = request.IsEnabled.Value; // samo ako je poslano

        await _ctx.SaveChangesAsync(ct);
        return Unit.Value;
    }
}