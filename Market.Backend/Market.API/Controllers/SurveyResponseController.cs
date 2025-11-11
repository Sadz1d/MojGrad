using Market.Application.Modules.Surveys.SurveyResponses.Commands.Create;
using Market.Application.Modules.Surveys.SurveyResponses.Commands.Delete;
using Market.Application.Modules.Surveys.SurveyResponses.Queries.GetById;
using Market.Application.Modules.Surveys.SurveyResponses.Queries.List;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Market.API.Controllers;

[ApiController]
[Route("api/survey-responses")]
public sealed class SurveyResponsesController : ControllerBase
{
    private readonly ISender _sender;
    public SurveyResponsesController(ISender sender) => _sender = sender;

    // GET /api/survey-responses?search=&surveyId=&userId=&page=1&pageSize=20
    [HttpGet]
    public async Task<PageResult<ListSurveyResponsesQueryDto>> List(
        [FromQuery] ListSurveyResponsesQuery query,
        CancellationToken ct)
        => await _sender.Send(query, ct);

    [HttpGet("{id:int}")]
    public async Task<GetSurveyResponseByIdQueryDto> GetById(int id, CancellationToken ct)
    => await _sender.Send(new GetSurveyResponseByIdQuery { Id = id }, ct);

    [HttpPost]
    public async Task<int> Create([FromBody] CreateSurveyResponseCommand command, CancellationToken ct)
    => await _sender.Send(command, ct);
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _sender.Send(new DeleteSurveyResponseCommand { Id = id }, ct);
        return NoContent();
    }
}
