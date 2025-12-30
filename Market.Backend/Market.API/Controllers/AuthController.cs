//using Market.Application.Modules.Auth.Commands.Login;
//using Market.Application.Modules.Auth.Commands.Logout;
//using Market.Application.Modules.Auth.Commands.Refresh;

//[ApiController]
//[Route("api/auth")]
//public sealed class AuthController(IMediator mediator) : ControllerBase
//{
//    [HttpPost("login")]
//    [AllowAnonymous]
//    public async Task<ActionResult<LoginCommandDto>> Login([FromBody] LoginCommand command, CancellationToken ct)
//    {
//        return Ok(await mediator.Send(command, ct));
//    }

//    [HttpPost("refresh")]
//    [AllowAnonymous]
//    public async Task<ActionResult<LoginCommandDto>> Refresh([FromBody] RefreshTokenCommand command, CancellationToken ct)
//    {
//        return Ok(await mediator.Send(command, ct));
//    }

//    [Authorize]
//    [HttpPost("logout")]
//    public async Task Logout([FromBody] LogoutCommand command, CancellationToken ct)
//    {
//        await mediator.Send(command, ct);
//    }
//}
using Market.Application.Modules.Auth.Commands.Login;
using Market.Application.Modules.Auth.Commands.Refresh;
using Market.Application.Modules.Auth.Commands.Logout;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Market.Api.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Prijava korisnika u MojGrad sistem
    /// (građanin, admin ili uposlenik)
    /// </summary>
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<LoginCommandDto>> Login(
        [FromBody] LoginCommand command,
        CancellationToken ct)
    {
        return Ok(await mediator.Send(command, ct));
    }

    /// <summary>
    /// Obnavljanje access tokena pomoću refresh tokena
    /// </summary>
    [HttpPost("refresh")]
    [AllowAnonymous]
    public async Task<ActionResult<LoginCommandDto>> Refresh(
        [FromBody] RefreshTokenCommand command,
        CancellationToken ct)
    {
        return Ok(await mediator.Send(command, ct));
    }

    /// <summary>
    /// Odjava korisnika iz MojGrad sistema
    /// </summary>
    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout(
        [FromBody] LogoutCommand command,
        CancellationToken ct)
    {
        await mediator.Send(command, ct);
        return NoContent();
    }
}
