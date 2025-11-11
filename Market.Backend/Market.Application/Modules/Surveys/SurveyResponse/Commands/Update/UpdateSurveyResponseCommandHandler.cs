using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Application.Common.Exceptions;

namespace Market.Application.Modules.Surveys.SurveyResponses.Commands.Update;

public sealed class UpdateSurveyResponseCommandHandler
    : IRequestHandler<UpdateSurveyResponseCommand, Unit>
{
    private readonly IAppDbContext _ctx;
    public UpdateSurveyResponseCommandHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<Unit> Handle(UpdateSurveyResponseCommand request, CancellationToken ct)
    {
        var entity = await _ctx.SurveyResponses
            .Include(r => r.Survey)
            .FirstOrDefaultAsync(r => r.Id == request.Id, ct);

        if (entity is null)
            throw new MarketNotFoundException($"SurveyResponse (Id={request.Id}) not found.");

        // (opcija) ne dozvoli izmjene nakon završetka ankete
        if (entity.Survey.EndDate < DateTime.UtcNow)
            throw new MarketConflictException("Cannot update a response after the survey has ended.");

        if (request.ResponseText is not null)
        {
            var text = request.ResponseText.Trim();
            if (string.IsNullOrWhiteSpace(text))
                throw new ArgumentException("ResponseText cannot be empty.");
            if (text.Length > 1000) // Constraints.ResponseMaxLength
                throw new ArgumentException("ResponseText exceeds 1000 characters.");
            entity.ResponseText = text;
        }

        await _ctx.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
