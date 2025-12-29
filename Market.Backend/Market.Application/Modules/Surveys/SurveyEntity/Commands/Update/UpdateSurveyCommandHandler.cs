using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Application.Common.Exceptions;

namespace Market.Application.Modules.Surveys.Survey.Commands.Update;

public sealed class UpdateSurveyCommandHandler
    : IRequestHandler<UpdateSurveyCommand, Unit>
{
    private readonly IAppDbContext _ctx;
    public UpdateSurveyCommandHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<Unit> Handle(UpdateSurveyCommand request, CancellationToken ct)
    {
        var entity = await _ctx.Surveys
            .FirstOrDefaultAsync(s => s.Id == request.Id, ct);

        if (entity is null)
            throw new MarketNotFoundException($"Survey (Id={request.Id}) not found.");

        if (request.Question is not null)
        {
            var q = request.Question.Trim();
            if (string.IsNullOrWhiteSpace(q))
                throw new ArgumentException("Question is required.");

            entity.Question = q;
        }

        // pripremi finalne datume (ako nisu poslani, uzmi postojeće)
        var newStart = request.StartDate ?? entity.StartDate;
        var newEnd = request.EndDate ?? entity.EndDate;

        if (newEnd <= newStart)
            throw new ArgumentException("Question is required.");


        // postavi samo ono što je poslano
        if (request.StartDate.HasValue) entity.StartDate = request.StartDate.Value;
        if (request.EndDate.HasValue) entity.EndDate = request.EndDate.Value;

        await _ctx.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
