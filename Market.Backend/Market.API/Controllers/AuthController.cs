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
using Market.Application.Modules.Auth.Commands.ForgotPassword;
using Market.Application.Modules.Auth.Commands.Login;
using Market.Application.Modules.Auth.Commands.Logout;
using Market.Application.Modules.Auth.Commands.Refresh;
using Market.Application.Modules.Auth.Commands.Register;
using Market.Application.Modules.Auth.Commands.ResetPassword;
using Market.Application.Modules.Identity.Users.Queries.GetById;
using Market.Application.Modules.Identity.Users.Queries.GetCurrentUser;
using Market.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Market.Api.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController(IMediator mediator) : ControllerBase
{

    //UserManager<ApplicationUser> userManager)
    //_userManager = userManager;

    /// <summary>
    /// Prijava korisnika u MojGrad sistem
    /// (građanin, admin ili uposlenik)
    /// </summary>
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(
         [FromBody] LoginCommand command,
         CancellationToken ct)
    {
        return Ok(await mediator.Send(command, ct));
    }

    /// <summary>
    /// Registracija novog korisnika
    /// </summary>
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<ActionResult<RegisterCommandDto>> Register(
        [FromBody] RegisterCommand command,
        CancellationToken ct)
    {
        return Ok(await mediator.Send(command, ct));
    }

    /// <summary>
    /// Zaboravljena lozinka
    /// </summary>
    [HttpPost("forgot-password")]
    [AllowAnonymous]
    public async Task<ActionResult<ForgotPasswordCommandDto>> ForgotPassword(
        [FromBody] ForgotPasswordCommand command,
        CancellationToken ct)
    {
        return Ok(await mediator.Send(command, ct));
    }

    /// <summary>
    /// Reset lozinke pomoću tokena poslanog na email
    /// </summary>
    [HttpPost("reset-password")]
    [AllowAnonymous]
    public async Task<IActionResult> ResetPassword(
        [FromBody] ResetPasswordCommand command,
        CancellationToken ct)
    {
        await mediator.Send(command, ct);
        return Ok();
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
    /// <summary>
    /// Get current user info
    /// </summary>
    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> GetCurrentUser(CancellationToken ct)
    {
        // 1️⃣ Uzmi userId iz JWT (sub claim)
        var userIdClaim = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
                          ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!int.TryParse(userIdClaim, out var userId))
            return Unauthorized();

        // 2️⃣ Pošalji GetMarketUserByIdQuery preko MediatR
        var query = new GetMarketUserByIdQuery
        {
            Id = userId
        };
        var user = await mediator.Send(query, ct);


        // 3️⃣ Mapiraj DTO u JSON koji frontend očekuje
        return Ok(new
        {
            Id = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            FullName = string.IsNullOrEmpty(user.FirstName + user.LastName)
                        ? null
                        : $"{user.FirstName} {user.LastName}".Trim(),
            IsAdmin = user.IsAdmin,
            IsManager = user.IsManager,
            IsEmployee = user.IsEmployee
        });
    }

}

