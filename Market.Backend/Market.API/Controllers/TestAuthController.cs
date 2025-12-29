using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Market.Application.Abstractions;

namespace Market.API.Controllers;

[ApiController]
[Route("api/test-auth")]
public class TestAuthController : ControllerBase
{
    private readonly IAppCurrentUser _currentUser;

    public TestAuthController(IAppCurrentUser currentUser)
    {
        _currentUser = currentUser;
    }

    // ? Bez tokena – mora pasti
    [HttpGet("public")]
    public IActionResult Public()
    {
        return Ok("Public endpoint – no auth needed");
    }

    // ?? Bilo koji prijavljeni korisnik
    [Authorize]
    [HttpGet("authenticated")]
    public IActionResult Authenticated()
    {
        return Ok(new
        {
            Message = "Authenticated user",
            _currentUser.UserId,
            _currentUser.Email,
            _currentUser.IsAdmin,
            _currentUser.IsManager,
            _currentUser.IsEmployee
        });
    }

    // ?? Samo ADMIN
    [Authorize(Roles = "Admin")]
    [HttpGet("admin")]
    public IActionResult AdminOnly()
    {
        return Ok("ADMIN ACCESS GRANTED");
    }

    // ????? Manager ILI Admin
    [Authorize(Roles = "Manager,Admin")]
    [HttpGet("manager")]
    public IActionResult ManagerOrAdmin()
    {
        return Ok("MANAGER OR ADMIN ACCESS");
    }

    // ?? Employee
    [Authorize(Roles = "Employee")]
    [HttpGet("employee")]
    public IActionResult EmployeeOnly()
    {
        return Ok("EMPLOYEE ACCESS");
    }
}
