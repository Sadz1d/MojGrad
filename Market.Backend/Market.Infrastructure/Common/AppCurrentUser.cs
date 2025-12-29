using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Market.Application.Abstractions;

namespace Market.Infrastructure.Common;

/// <summary>
/// Reads current user info from JWT claims (standard ASP.NET Core roles)
/// </summary>
public sealed class AppCurrentUser(IHttpContextAccessor httpContextAccessor)
    : IAppCurrentUser
{
    private readonly ClaimsPrincipal? _user = httpContextAccessor.HttpContext?.User;

    public int? UserId =>
        int.TryParse(_user?.FindFirstValue(ClaimTypes.NameIdentifier), out var id)
            ? id
            : null;

    public string? Email =>
        _user?.FindFirstValue(ClaimTypes.Email);

    public bool IsAuthenticated =>
        _user?.Identity?.IsAuthenticated ?? false;

    // ✅ STANDARD ROLE CHECKS
    public bool IsAdmin =>
        _user?.IsInRole("Admin") ?? false;

    public bool IsManager =>
        _user?.IsInRole("Manager") ?? false;

    public bool IsEmployee =>
        _user?.IsInRole("Employee") ?? false;
}
