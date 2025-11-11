using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Application.Common.Exceptions;

namespace Market.Application.Modules.Surveys.Survey.Queries.GetById;

public sealed class GetSurveyByIdQueryHandler
    : IRequestHandler<GetSurveyByIdQuery, GetSurveyByIdQueryDto>
{
    private readonly IAppDbContext _ctx;
    public GetSurveyByIdQueryHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<GetSurveyByIdQueryDto> Handle(
        GetSurveyByIdQuery request, CancellationToken ct)
    {
        var dto = await _ctx.Surveys
            .AsNoTracking()
            .Where(s => s.Id == request.Id)
            .Select(s => new GetSurveyByIdQueryDto
            {
                Id = s.Id,
                Question = s.Question,
                StartDate = s.StartDate,
                EndDate = s.EndDate,
                ResponsesCount = _ctx.SurveyResponses.Count(r => r.SurveyId == s.Id),
                IsActive = s.StartDate <= DateTime.UtcNow && s.EndDate >= DateTime.UtcNow
            })
            .FirstOrDefaultAsync(ct);

        if (dto is null)
            throw new MarketNotFoundException($"Survey with Id {request.Id} not found.");

        return dto;
    }
}
