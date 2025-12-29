using MediatR;
using Microsoft.EntityFrameworkCore;
//using Market.Infrastructure.Database;

namespace Market.Application.Modules.Surveys.Commands.Status.Disable;

public sealed class DisableSurveyCommandHandler(IAppDbContext ctx)
    : IRequestHandler<DisableSurveyCommand, Unit>
{
    public async Task<Unit> Handle(DisableSurveyCommand request, CancellationToken ct)
    {
        var survey = await ctx.Surveys
            .FirstOrDefaultAsync(x => x.Id == request.Id, ct);

        if (survey is null)
        {
            throw new MarketNotFoundException(
                $"Survey (ID={request.Id}) nije pronađen.");
        }

        if (!survey.IsEnabled)
            return Unit.Value; // idempotent

        survey.IsEnabled = false;

        await ctx.SaveChangesAsync(ct);

        return Unit.Value;
    }
}
