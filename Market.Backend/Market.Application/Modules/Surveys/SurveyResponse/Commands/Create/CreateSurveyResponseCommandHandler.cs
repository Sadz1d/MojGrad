using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Application.Common.Exceptions;
using Market.Domain.Entities.Surveys;

namespace Market.Application.Modules.Surveys.SurveyResponses.Commands.Create;

public sealed class CreateSurveyResponseCommandHandler
    : IRequestHandler<CreateSurveyResponseCommand, int>
{
    private readonly IAppDbContext _ctx;
    public CreateSurveyResponseCommandHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<int> Handle(CreateSurveyResponseCommand request, CancellationToken ct)
    {
        // 1) Validacije postojanja
        var surveyExists = await _ctx.Surveys.AnyAsync(s => s.Id == request.SurveyId, ct);
        if (!surveyExists)
            throw new MarketNotFoundException($"Survey with ID {request.SurveyId} not found.");

        var userExists = await _ctx.Users.AnyAsync(u => u.Id == request.UserId, ct);
        if (!userExists)
            throw new MarketNotFoundException($"User with ID {request.UserId} not found.");

        // 2) (Opcionalno) zabrani dupli odgovor istog korisnika na istu anketu
        var duplicate = await _ctx.SurveyResponses
            .AnyAsync(r => r.SurveyId == request.SurveyId && r.UserId == request.UserId, ct);
        if (duplicate)
            throw new MarketConflictException("User has already submitted a response for this survey.");

        // 3) Kreiraj entitet
        var entity = new SurveyResponseEntity
        {
            SurveyId = request.SurveyId,
            UserId = request.UserId,
            ResponseText = request.ResponseText.Trim()
        };

        _ctx.SurveyResponses.Add(entity);
        await _ctx.SaveChangesAsync(ct);
        return entity.Id;
    }
}
