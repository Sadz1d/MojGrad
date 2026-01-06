// Market.Application/Modules/Auth/Commands/Register/RegisterCommandDto.cs
namespace Market.Application.Modules.Auth.Commands.Register;

/// <summary>
/// Response after successful registration
/// </summary>
public sealed class RegisterCommandDto
{
    public int UserId { get; set; }
    public string Email { get; set; }
    public string FullName { get; set; }
    public string Message { get; set; }
}