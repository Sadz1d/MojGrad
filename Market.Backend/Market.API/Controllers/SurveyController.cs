using Market.Application.Modules.Surveys.Commands.Status.Disable;
using Market.Application.Modules.Surveys.Commands.Status.Enable;
using Market.Application.Modules.Surveys.Survey.Commands.Create;
using Market.Application.Modules.Surveys.Survey.Commands.Delete;
using Market.Application.Modules.Surveys.Survey.Commands.Update;
using Market.Application.Modules.Surveys.Survey.Queries.GetById;
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

    [HttpGet("{id:int}")]
    public async Task<GetSurveyByIdQueryDto> GetById(int id, CancellationToken ct)
    => await _sender.Send(new GetSurveyByIdQuery { Id = id }, ct);


    // CREATE
    [HttpPost]
    public async Task<int> Create([FromBody] CreateSurveyCommand cmd, CancellationToken ct)
        => await _sender.Send(cmd, ct);

    // DELETE
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _sender.Send(new DeleteSurveyCommand { Id = id }, ct);
        return NoContent();
    }
    // UPDATE
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateSurveyCommand cmd, CancellationToken ct)
    {
        cmd.Id = id;
        await _sender.Send(cmd, ct);
        return NoContent();
    }
    // DISABLE
    [HttpPut("{id:int}/disable")]
    public async Task<IActionResult> Disable(int id, CancellationToken ct)
    {
        await _sender.Send(new DisableSurveyCommand { Id = id }, ct);
        return NoContent();
    }

    // ENABLE
    [HttpPut("{id:int}/enable")]
    public async Task<IActionResult> Enable(int id, CancellationToken ct)
    {
        await _sender.Send(new EnableSurveyCommand { Id = id }, ct);
        return NoContent();
    }
}
