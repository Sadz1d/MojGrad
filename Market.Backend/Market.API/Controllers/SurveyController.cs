using Market.Application.Modules.Surveys.Survey.Queries.List;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Market.API.Controllers;

[ApiController]
[Route("api/surveys")]
public sealed class SurveyController : ControllerBase
{
    private readonly ISender _sender;
    public SurveyController(ISender sender) => _sender = sender;

    // GET /api/surveys?search=&activeOn=&page=1&pageSize=20
    [HttpGet]
    public async Task<PageResult<ListSurveysQueryDto>> List(
        [FromQuery] ListSurveysQuery query,
        CancellationToken ct)
        => await _sender.Send(query, ct);
}
