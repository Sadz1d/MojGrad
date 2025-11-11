using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Domain.Entities.Surveys;

namespace Market.Application.Modules.Surveys.SurveyResponses.Queries.List;

public sealed class ListSurveyResponsesQueryHandler
    : IRequestHandler<ListSurveyResponsesQuery, PageResult<ListSurveyResponsesQueryDto>>
{
    private readonly IAppDbContext _ctx;
    public ListSurveyResponsesQueryHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<PageResult<ListSurveyResponsesQueryDto>> Handle(
        ListSurveyResponsesQuery request, CancellationToken ct)
    {
        IQueryable<SurveyResponseEntity> q = _ctx.SurveyResponses
            .AsNoTracking()
            .Include(r => r.User)
            .Include(r => r.Survey);

        // 🔍 Pretraga
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var term = request.Search.Trim().ToLower();
            q = q.Where(r =>
                r.ResponseText.ToLower().Contains(term) ||
                (r.User.FirstName + " " + r.User.LastName).ToLower().Contains(term) ||
                r.Survey.Question.ToLower().Contains(term));
        }

        // 🧩 Filteri
        if (request.SurveyId.HasValue)
            q = q.Where(r => r.SurveyId == request.SurveyId.Value);

        if (request.UserId.HasValue)
            q = q.Where(r => r.UserId == request.UserId.Value);

        // 📊 Projekcija
        var projected = q
            .OrderByDescending(r => r.Id)
            .Select(r => new ListSurveyResponsesQueryDto
            {
                Id = r.Id,
                SurveyId = r.SurveyId,
                SurveyQuestion = r.Survey.Question,
                UserName = (r.User.FirstName + " " + r.User.LastName).Trim(),
                ResponseText = r.ResponseText
            });

        return await PageResult<ListSurveyResponsesQueryDto>
            .FromQueryableAsync(projected, request.Paging, ct);
    }
}
