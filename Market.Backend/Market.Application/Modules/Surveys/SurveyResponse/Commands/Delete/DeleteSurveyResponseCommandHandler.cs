using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Application.Common.Exceptions;
using Market.Domain.Entities.Surveys;

namespace Market.Application.Modules.Surveys.SurveyResponses.Commands.Delete;

public sealed class DeleteSurveyResponseCommandHandler
    : IRequestHandler<DeleteSurveyResponseCommand, Unit>
{
    private readonly IAppDbContext _ctx;
    public DeleteSurveyResponseCommandHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<Unit> Handle(DeleteSurveyResponseCommand request, CancellationToken ct)
    {
        var entity = await _ctx.SurveyResponses
            .FirstOrDefaultAsync(x => x.Id == request.Id, ct);

        if (entity == null)
            throw new MarketNotFoundException($"SurveyResponse with Id {request.Id} not found.");

        _ctx.SurveyResponses.Remove(entity);
        await _ctx.SaveChangesAsync(ct);

        return Unit.Value;
    }
}
