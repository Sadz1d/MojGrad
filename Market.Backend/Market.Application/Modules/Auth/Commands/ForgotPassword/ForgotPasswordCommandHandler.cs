// Market.Application/Modules/Auth/Commands/ForgotPassword/ForgotPasswordCommandHandler.cs
using Market.Application.Abstractions;
using Market.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using MediatR;

namespace Market.Application.Modules.Auth.Commands.ForgotPassword;

public sealed class ForgotPasswordCommandHandler(
    IAppDbContext ctx,
    TimeProvider timeProvider)
    : IRequestHandler<ForgotPasswordCommand, ForgotPasswordCommandDto>
{
    public async Task<ForgotPasswordCommandDto> Handle(ForgotPasswordCommand request, CancellationToken ct)
    {
        var normalizedEmail = request.Email.Trim().ToLowerInvariant();
        var user = await ctx.Users
            .FirstOrDefaultAsync(x => x.Email.ToLower() == normalizedEmail, ct);

        // Security: Always return success even if user doesn't exist
        if (user == null)
        {
            return new ForgotPasswordCommandDto
            {
                Email = request.Email,
                Message = "Ako email postoji, poslali smo vam link za resetovanje lozinke.",
                ResetTokenExpiresAt = timeProvider.GetUtcNow().UtcDateTime.AddHours(24)
            };
        }

        // Generate reset token (simplified - in real app, generate proper token and send email)
        var resetToken = Guid.NewGuid().ToString("N");

        // TODO: Save reset token to database and send email
        // For now, we'll just return success message

        return new ForgotPasswordCommandDto
        {
            Email = user.Email,
            Message = "Poslali smo vam email sa uputstvima za resetovanje lozinke.",
            ResetTokenExpiresAt = timeProvider.GetUtcNow().UtcDateTime.AddHours(24)
        };
    }
}