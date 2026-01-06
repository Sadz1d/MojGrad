// Market.Application/Modules/Auth/Commands/ForgotPassword/ForgotPasswordCommand.cs
using MediatR;

namespace Market.Application.Modules.Auth.Commands.ForgotPassword;

/// <summary>
/// Command for initiating password reset
/// </summary>
public sealed class ForgotPasswordCommand : IRequest<ForgotPasswordCommandDto>
{
    public string Email { get; init; }
}