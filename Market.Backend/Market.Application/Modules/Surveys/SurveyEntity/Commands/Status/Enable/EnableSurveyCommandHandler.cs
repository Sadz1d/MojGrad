using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Market.Application.Modules.Surveys.Commands.Status.Enable;

public sealed class EnableSurveyCommandHandler(IAppDbContext ctx)
    : IRequestHandler<EnableSurveyCommand, Unit>
{
    public async Task<Unit> Handle(EnableSurveyCommand request, CancellationToken ct)
    {
        var survey = await ctx.Surveys
            .FirstOrDefaultAsync(x => x.Id == request.Id, ct);

        if (survey is null)
            throw new MarketNotFoundException(
                $"Survey (ID={request.Id}) nije pronađen.");

        if (!survey.IsEnabled)
        {
            survey.IsEnabled = true;
            await ctx.SaveChangesAsync(ct);
        }

        return Unit.Value; // idempotent
    }
}
