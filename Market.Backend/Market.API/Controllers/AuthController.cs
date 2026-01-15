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
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Market.Application.Modules.Auth.Commands.ResetPassword;
using Market.Domain.Entities;
using Microsoft.AspNetCore.Identity;


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
        // This would require a new query handler
        // For now, return basic info from claims
        var userId = User.FindFirst("sub")?.Value;
        var email = User.FindFirst(ClaimTypes.Email)?.Value;
        var fullName = $"{User.FindFirst("firstName")?.Value} {User.FindFirst("lastName")?.Value}".Trim();

        return Ok(new
        {
            Id = userId,
            Email = email,
            FullName = string.IsNullOrEmpty(fullName) ? null : fullName,
            IsAdmin = bool.TryParse(User.FindFirst("is_admin")?.Value, out var isAdmin) && isAdmin,
            IsManager = bool.TryParse(User.FindFirst("is_manager")?.Value, out var isManager) && isManager,
            IsEmployee = bool.TryParse(User.FindFirst("is_employee")?.Value, out var isEmployee) && isEmployee
        });
    }
}
