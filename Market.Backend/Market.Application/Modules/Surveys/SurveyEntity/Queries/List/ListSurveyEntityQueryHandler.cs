using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Domain.Entities.Surveys;

namespace Market.Application.Modules.Surveys.Survey.Queries.List;

public sealed class ListSurveysQueryHandler
    : IRequestHandler<ListSurveysQuery, PageResult<ListSurveysQueryDto>>
{
    private readonly IAppDbContext _ctx;
    public ListSurveysQueryHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<PageResult<ListSurveysQueryDto>> Handle(
        ListSurveysQuery request, CancellationToken ct)
    {
        IQueryable<SurveyEntity> q = _ctx.Surveys.AsNoTracking();

        // 🔍 pretraga po pitanju (Question)
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var term = request.Search.Trim().ToLower();
            q = q.Where(s => s.Question.ToLower().Contains(term));
        }

        // 📅 filtriranje po aktivnom periodu
        if (request.ActiveOn.HasValue)
        {
            var date = request.ActiveOn.Value;
            q = q.Where(s => s.StartDate <= date && s.EndDate >= date);
        }

        // sortiranje po datumu početka (novije ankete prve)
        var projected = q
            .OrderByDescending(s => s.StartDate)
            .Select(s => new ListSurveysQueryDto
            {
                Id = s.Id,
                Question = s.Question,
                StartDate = s.StartDate,
                EndDate = s.EndDate,
                ResponsesCount = _ctx.SurveyResponses.Count(r => r.SurveyId == s.Id),
                IsActive = s.StartDate <= DateTime.UtcNow && s.EndDate >= DateTime.UtcNow
            });

        return await PageResult<ListSurveysQueryDto>
            .FromQueryableAsync(projected, request.Paging, ct);
    }
}
