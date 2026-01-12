using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Market.Application.Abstractions;
using Market.Domain.Entities.Identity;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Market.Application.Modules.Auth.Commands.ResetPassword;

public sealed class ResetPasswordCommandHandler(
    IAppDbContext ctx,
    IPasswordHasher<MarketUserEntity> passwordHasher,
    TimeProvider timeProvider)
    : IRequestHandler<ResetPasswordCommand>
{
    public async Task Handle(ResetPasswordCommand request, CancellationToken ct)
    {
        var tokenEntity = await ctx.PasswordResetTokens
            .Include(x => x.User)
            .FirstOrDefaultAsync(x =>
                x.Token == request.Token &&
                !x.IsUsed &&
                x.ExpiresAt > timeProvider.GetUtcNow().UtcDateTime,
                ct);

        if (tokenEntity == null)
            throw new InvalidOperationException("Nevažeći ili istekao reset token.");

        // Hash nove lozinke
        tokenEntity.User.PasswordHash =
            passwordHasher.HashPassword(tokenEntity.User, request.NewPassword);

        // Oznaci token kao iskorišten
        tokenEntity.IsUsed = true;

        await ctx.SaveChangesAsync(ct);
    }
}

