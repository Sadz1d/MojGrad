using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Application.Common.Exceptions;
using Market.Domain.Entities.Surveys;

namespace Market.Application.Modules.Surveys.SurveyResponses.Queries.GetById;

public sealed class GetSurveyResponseByIdQueryHandler
    : IRequestHandler<GetSurveyResponseByIdQuery, GetSurveyResponseByIdQueryDto>
{
    private readonly IAppDbContext _ctx;
    public GetSurveyResponseByIdQueryHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<GetSurveyResponseByIdQueryDto> Handle(
        GetSurveyResponseByIdQuery request, CancellationToken ct)
    {
        var dto = await _ctx.SurveyResponses
            .AsNoTracking()
            .Include(r => r.Survey)
            .Include(r => r.User)
            .Where(r => r.Id == request.Id)
            .Select(r => new GetSurveyResponseByIdQueryDto
            {
                Id = r.Id,
                SurveyId = r.SurveyId,
                SurveyQuestion = r.Survey.Question,
                UserId = r.UserId,
                UserName = (r.User.FirstName + " " + r.User.LastName).Trim(),
                ResponseText = r.ResponseText
            })
            .FirstOrDefaultAsync(ct);

        if (dto is null)
            throw new MarketNotFoundException($"SurveyResponse with Id {request.Id} not found.");

        return dto;
    }
}
