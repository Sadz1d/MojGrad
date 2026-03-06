using Market.Application.Modules.Identity.Profiles.Commands.Create;
using Market.Application.Modules.Identity.Profiles.Commands.Delete;
using Market.Application.Modules.Identity.Profiles.Commands.Update;
using Market.Application.Modules.Identity.Profiles.Commands.UploadPicture;
using Market.Application.Modules.Identity.Profiles.Queries.GetById;
using Market.Application.Modules.Identity.Profiles.Queries.GetByUserId;
using Market.Application.Modules.Identity.Profiles.Queries.GetPicture;
using Market.Application.Modules.Identity.Profiles.Queries.List;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Market.API.Controllers.Identity;

[ApiController]
[Route("api/[controller]")]
public class ProfilesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProfilesController(IMediator mediator) => _mediator = mediator;

    [HttpGet("list")]
    public async Task<IActionResult> List([FromQuery] ListProfilesQuery query)
        => Ok(await _mediator.Send(query));

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
        => Ok(await _mediator.Send(new GetProfileByIdQuery { Id = id }));

    [HttpGet("by-user/{userId:int}")]
    public async Task<IActionResult> GetByUserId(int userId, CancellationToken ct)
        => Ok(await _mediator.Send(new GetProfileByUserIdQuery { UserId = userId }, ct));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProfileCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id }, null);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateProfileCommand command)
    {
        command.Id = id;
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPut("by-user/{userId:int}")]
    public async Task<IActionResult> UpdateByUserId(int userId, [FromBody] UpdateProfileCommand command, CancellationToken ct)
    {
        var profile = await _mediator.Send(new GetProfileByUserIdQuery { UserId = userId }, ct);
        command.Id = profile.Id;
        await _mediator.Send(command, ct);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _mediator.Send(new DeleteProfileCommand { Id = id });
        return NoContent();
    }

    [HttpPost("by-user/{userId:int}/upload-picture")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadPicture(int userId, IFormFile image, CancellationToken ct)
    {
        var imageUrl = await _mediator.Send(
            new UploadProfilePictureCommand { UserId = userId, Image = image }, ct);
        return Ok(new { imageUrl });
    }

    [HttpGet("by-user/{userId:int}/picture")]
    public async Task<IActionResult> GetPicture(int userId, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetProfilePictureQuery { UserId = userId }, ct);
        return PhysicalFile(result.FilePath, result.MimeType);
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> GetMyProfile(CancellationToken ct)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
        if (userId == null) return Unauthorized();
        return Ok(await _mediator.Send(new GetProfileByIdQuery { Id = int.Parse(userId) }, ct));
    }
}