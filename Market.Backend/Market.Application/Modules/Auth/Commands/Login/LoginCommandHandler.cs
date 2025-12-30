//using Market.Application.Modules.Auth.Commands.Login;

//public sealed class LoginCommandHandler(
//    IAppDbContext ctx,
//    IJwtTokenService jwt,
//    IPasswordHasher<MarketUserEntity> hasher)
//    : IRequestHandler<LoginCommand, LoginCommandDto>
//{
//    public async Task<LoginCommandDto> Handle(LoginCommand request, CancellationToken ct)
//    {
//        var email = request.Email.Trim().ToLowerInvariant();

//        var user = await ctx.Users
//            .FirstOrDefaultAsync(x => x.Email.ToLower() == email && x.IsEnabled && !x.IsDeleted, ct)
//            ?? throw new MarketNotFoundException("Korisnik nije pronađen ili je onemogućen.");

//        var verify = hasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
//        if (verify == PasswordVerificationResult.Failed)
//            throw new MarketConflictException("Pogrešni kredencijali.");

//        var tokens = jwt.IssueTokens(user);

//        ctx.RefreshTokens.Add(new RefreshTokenEntity
//        {
//            TokenHash = tokens.RefreshTokenHash,
//            ExpiresAtUtc = tokens.RefreshTokenExpiresAtUtc,
//            UserId = user.Id,
//            Fingerprint = request.Fingerprint
//        });

//        await ctx.SaveChangesAsync(ct);

//        return new LoginCommandDto
//        {
//            AccessToken = tokens.AccessToken,
//            RefreshToken = tokens.RefreshTokenRaw,
//            ExpiresAtUtc = tokens.RefreshTokenExpiresAtUtc
//        };
//    }
//}
using Market.Application.Abstractions;
using Market.Domain.Entities.Identity;
//using Market.Domain.Entities.Identity.RefreshToken;
//using Market.Application.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MediatR;

namespace Market.Application.Modules.Auth.Commands.Login;

public sealed class LoginCommandHandler(
    IAppDbContext ctx,
    IJwtTokenService jwt,
    IPasswordHasher<MarketUserEntity> hasher)
    : IRequestHandler<LoginCommand, LoginCommandDto>
{
    public async Task<LoginCommandDto> Handle(LoginCommand request, CancellationToken ct)
    {
        // 1️⃣ Normalizacija emaila (MOJGRAD login)
        var email = request.Email.Trim().ToLowerInvariant();

        // 2️⃣ Dohvat korisnika (građanin / admin / uposlenik)
        var user = await ctx.Users
            .FirstOrDefaultAsync(x =>
                x.Email.ToLower() == email &&
                x.IsEnabled &&
                !x.IsDeleted, ct)
            ?? throw new MarketNotFoundException(
                "Korisnik ne postoji ili je onemogućen.");

        // 3️⃣ Provjera lozinke
        var verify = hasher.VerifyHashedPassword(
            user,
            user.PasswordHash,
            request.Password);

        if (verify == PasswordVerificationResult.Failed)
            throw new MarketConflictException(
                "Pogrešni kredencijali.");

        // 4️⃣ Generisanje JWT + Refresh tokena (MOJGRAD)
        var tokens = jwt.IssueTokens(user);

        // 5️⃣ Spremanje refresh tokena (sigurnost)
        ctx.RefreshTokens.Add(new RefreshTokenEntity
        {
            TokenHash = tokens.RefreshTokenHash,
            ExpiresAtUtc = tokens.RefreshTokenExpiresAtUtc,
            UserId = user.Id,
            Fingerprint = request.Fingerprint
        });

        await ctx.SaveChangesAsync(ct);

        // 6️⃣ Povrat tokena klijentu
        return new LoginCommandDto
        {
            AccessToken = tokens.AccessToken,
            RefreshToken = tokens.RefreshTokenRaw,
            ExpiresAtUtc = tokens.AccessTokenExpiresAtUtc
        };
    }
}
