// Market.Application/Modules/Auth/Commands/ForgotPassword/ForgotPasswordCommandDto.cs
namespace Market.Application.Modules.Auth.Commands.ForgotPassword;

public sealed class ForgotPasswordCommandDto
{
    public string Email { get; set; }
    public string Message { get; set; }
    public DateTime ResetTokenExpiresAt { get; set; }
}