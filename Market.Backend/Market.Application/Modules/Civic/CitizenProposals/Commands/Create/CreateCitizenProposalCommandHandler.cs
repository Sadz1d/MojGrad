using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Domain.Entities.Civic;
using Market.Application.Common.Exceptions;

namespace Market.Application.Modules.Civic.CitizenProposals.Commands.Create;

public sealed class CreateCitizenProposalCommandHandler
    : IRequestHandler<CreateCitizenProposalCommand, int>
{
    private readonly IAppDbContext _ctx;
    public CreateCitizenProposalCommandHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<int> Handle(CreateCitizenProposalCommand request, CancellationToken ct)
    {
        var normalizedTitle = request.Title.Trim();

        // (opcija) zabrani duplikat naslova za istog korisnika
        var exists = await _ctx.CitizenProposals
            .AnyAsync(p => p.UserId == request.UserId && p.Title == normalizedTitle, ct);
        if (exists)
            throw new MarketConflictException("Proposal with the same title already exists for this user.");

        var entity = new CitizenProposalEntity
        {
            UserId = request.UserId,
            Title = normalizedTitle,
            Text = request.Text.Trim(),
            PublicationDate = DateTime.UtcNow,
            IsEnabled = request.IsEnabled ?? true
        };

        _ctx.CitizenProposals.Add(entity);
        await _ctx.SaveChangesAsync(ct);

        return entity.Id;
    }
}