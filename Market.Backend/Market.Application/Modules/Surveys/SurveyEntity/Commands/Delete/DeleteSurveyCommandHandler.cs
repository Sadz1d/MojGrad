using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Application.Common.Exceptions;

namespace Market.Application.Modules.Surveys.Survey.Commands.Delete;

public sealed class DeleteSurveyCommandHandler
    : IRequestHandler<DeleteSurveyCommand, Unit>
{
    private readonly IAppDbContext _ctx;
    public DeleteSurveyCommandHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<Unit> Handle(DeleteSurveyCommand request, CancellationToken ct)
    {
        var entity = await _ctx.Surveys
            .FirstOrDefaultAsync(s => s.Id == request.Id, ct);

        if (entity is null)
            throw new MarketNotFoundException($"Survey with Id {request.Id} not found.");

        // Ako ne želite brisati ankete koje već imaju odgovore:
        var hasResponses = await _ctx.SurveyResponses.AnyAsync(r => r.SurveyId == entity.Id, ct);
        if (hasResponses)
            throw new MarketConflictException("Cannot delete a survey that already has responses.");

        _ctx.Surveys.Remove(entity);
        await _ctx.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
