using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Domain.Entities.Surveys;

namespace Market.Application.Modules.Surveys.Survey.Queries.List;

public sealed class ListSurveysQueryHandler
    : IRequestHandler<ListSurveysQuery, PageResult<ListSurveysQueryDto>>
{
    private readonly IAppDbContext _ctx;

    public ListSurveysQueryHandler(IAppDbContext ctx)
        => _ctx = ctx;

    public async Task<PageResult<ListSurveysQueryDto>> Handle(
        ListSurveysQuery request,
        CancellationToken ct)
    {
        IQueryable<SurveyEntity> q = _ctx.Surveys.AsNoTracking();

        // ===============================
        // 1️⃣ SEARCH (tekstualna pretraga)
        // ===============================
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var term = request.Search.Trim();

            // BE VALIDACIJA
            if (term.Length < 3)
                throw new ArgumentException("Search mora imati najmanje 3 znaka.");

            q = q.Where(s => s.Question.Contains(term));
        }

        // =====================================
        // 2️⃣ ACTIVE ON (aktivna na određeni dan)
        // =====================================
        if (request.ActiveOn.HasValue)
        {
            var date = request.ActiveOn.Value;
            q = q.Where(s => s.StartDate <= date && s.EndDate >= date);
        }

        // =====================================
        // 3️⃣ ONLY ACTIVE (samo trenutno aktivne)
        // =====================================
        if (request.OnlyActive == true)
        {
            var now = DateTime.Today;
            q = q.Where(s =>
                s.IsEnabled &&
                s.StartDate <= now &&
                s.EndDate >= now);
        }

        // =========================
        // 4️⃣ FROM DATE (od datuma)
        // =========================
        if (request.FromDate.HasValue)
        {
            q = q.Where(s => s.EndDate >= request.FromDate.Value);
        }

        // =========================
        // 5️⃣ TO DATE (do datuma)
        // =========================
        if (request.ToDate.HasValue)
        {
            q = q.Where(s => s.StartDate <= request.ToDate.Value);
        }

        // =========================
        // PROJECTION + SORT
        // =========================
        var projected = q
            .OrderByDescending(s => s.StartDate)
            .Select(s => new ListSurveysQueryDto
            {
                Id = s.Id,
                Question = s.Question,
                StartDate = s.StartDate,
                EndDate = s.EndDate,
                ResponsesCount = _ctx.SurveyResponses.Count(r => r.SurveyId == s.Id),
                IsActive =
                    s.IsEnabled &&
                    s.StartDate <= DateTime.Today &&
                    s.EndDate >= DateTime.Today
            });

        return await PageResult<ListSurveysQueryDto>
            .FromQueryableAsync(projected, request.Paging, ct);
    }
}
