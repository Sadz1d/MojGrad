using Market.Application.Modules.Volunteering.Commands.Status.Disable;
using Market.Application.Modules.Volunteering.Commands.Status.Enable;
using Market.Application.Modules.Volunteering.VolunteerActions.Commands.Create;
using Market.Application.Modules.Volunteering.VolunteerActions.Commands.Delete;
using Market.Application.Modules.Volunteering.VolunteerActions.Commands.Update;
using Market.Application.Modules.Volunteering.VolunteerActions.Queries.GetById;
using Market.Application.Modules.Volunteering.VolunteerActions.Queries.List;
using Market.Application.Modules.Volunteering.ActionParticipants.Commands.Create;
using Microsoft.AspNetCore.Authorization;
using MediatR;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace Market.API.Controllers;

[ApiController]
[Route("api/volunteering/actions")]
public sealed class VolunteerActionsController : ControllerBase
{
    private readonly ISender _sender;
    public VolunteerActionsController(ISender sender) => _sender = sender;

    // GET /api/volunteering/actions?search=&dateFrom=&dateTo=&onlyUpcoming=&onlyWithFreeSlots=&page=1&pageSize=20
    [HttpGet]
    public async Task<PageResult<ListVolunteerActionsQueryDto>> List(
        [FromQuery] ListVolunteerActionsQuery query,
        CancellationToken ct)
        => await _sender.Send(query, ct);

    [HttpGet("{id:int}")]
    public async Task<GetVolunteerActionByIdQueryDto> GetById(int id, CancellationToken ct)
    => await _sender.Send(new GetVolunteerActionByIdQuery { Id = id }, ct);

    [HttpPost("{id:int}/join")]
    [Authorize]
    public async Task<int> Join(int id, CancellationToken ct)
    {
        var userIdClaim =
            User.FindFirst("UserId")?.Value
            ?? User.FindFirst("userId")?.Value
            ?? User.FindFirst("id")?.Value
            ?? User.FindFirst("nameid")?.Value
            ?? User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrWhiteSpace(userIdClaim))
            throw new UnauthorizedAccessException("User ID nije pronađen u tokenu.");

        if (!int.TryParse(userIdClaim, out var userId))
            throw new UnauthorizedAccessException($"User ID iz tokena nije broj: {userIdClaim}");

        return await _sender.Send(new CreateActionParticipantCommand
        {
            ActionId = id,
            UserId = userId
        }, ct);
    }

    [HttpPost]
    public async Task<int> Create([FromBody] CreateVolunteerActionCommand command, CancellationToken ct)
    => await _sender.Send(command, ct);

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _sender.Send(new DeleteVolunteerActionCommand { Id = id }, ct);
        return NoContent();
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateVolunteerActionCommand command, CancellationToken ct)
    {
        command.Id = id;
        await _sender.Send(command, ct);
        return NoContent();
    }
    // DISABLE
    [HttpPut("{id:int}/disable")]
    public async Task<IActionResult> Disable(int id, CancellationToken ct)
    {
        await _sender.Send(new DisableVolunteerActionCommand { Id = id }, ct);
        return NoContent();
    }

    // ENABLE
    [HttpPut("{id:int}/enable")]
    public async Task<IActionResult> Enable(int id, CancellationToken ct)
    {
        await _sender.Send(new EnableVolunteerActionCommand { Id = id }, ct);
        return NoContent();
    }
}
