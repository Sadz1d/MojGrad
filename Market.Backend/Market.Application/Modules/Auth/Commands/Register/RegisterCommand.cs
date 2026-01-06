// Market.Application/Modules/Auth/Commands/Register/RegisterCommand.cs
using MediatR;

namespace Market.Application.Modules.Auth.Commands.Register;

/// <summary>
/// Command for user registration in MojGrad system
/// </summary>
public sealed class RegisterCommand : IRequest<RegisterCommandDto>
{
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string Email { get; init; }
    public string Password { get; init; }
    public string ConfirmPassword { get; init; }
    public string? PhoneNumber { get; init; }
}