using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Market.Application.Modules.Volunteering.Commands.Status.Enable;

public sealed class EnableVolunteerActionCommandHandler(IAppDbContext ctx)
    : IRequestHandler<EnableVolunteerActionCommand, Unit>
{
    public async Task<Unit> Handle(EnableVolunteerActionCommand request, CancellationToken ct)
    {
        var action = await ctx.VolunteerActions
            .FirstOrDefaultAsync(x => x.Id == request.Id, ct);

        if (action is null)
        {
            throw new MarketNotFoundException(
                $"Volunteer action (ID={request.Id}) nije pronađena.");
        }

        if (!action.IsEnabled)
        {
            action.IsEnabled = true;
            await ctx.SaveChangesAsync(ct);
        }

        return Unit.Value; // idempotent
    }
}
