// Market.Application/Modules/Auth/Commands/Register/RegisterCommandHandler.cs
using Market.Application.Abstractions;
using Market.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MediatR;

namespace Market.Application.Modules.Auth.Commands.Register;

public sealed class RegisterCommandHandler(
    IAppDbContext ctx,
    IPasswordHasher<MarketUserEntity> hasher,
    TimeProvider timeProvider)
    : IRequestHandler<RegisterCommand, RegisterCommandDto>
{
    public async Task<RegisterCommandDto> Handle(RegisterCommand request, CancellationToken ct)
    {
        // 1️⃣ Validacija lozinki
        if (request.Password != request.ConfirmPassword)
            throw new MarketConflictException("Lozinke se ne podudaraju.");

        // 2️⃣ Provjera da li email već postoji
        var normalizedEmail = request.Email.Trim().ToLowerInvariant();
        var existingUser = await ctx.Users
            .FirstOrDefaultAsync(x => x.Email.ToLower() == normalizedEmail, ct);

        if (existingUser != null)
            throw new MarketConflictException("Email je već registriran.");

        // 3️⃣ Kreiranje novog korisnika
        var user = new MarketUserEntity
        {
            FirstName = request.FirstName.Trim(),
            LastName = request.LastName.Trim(),
            Email = normalizedEmail,
            PasswordHash = hasher.HashPassword(null!, request.Password),
            IsAdmin = false,
            IsManager = false,
            IsEmployee = false,
            IsEnabled = true,
            TokenVersion = 0,
            RegistrationDate = timeProvider.GetUtcNow().UtcDateTime,
            Points = 0,
            CreatedAtUtc = timeProvider.GetUtcNow().UtcDateTime
        };

        // 4️⃣ Dodavanje u bazu
        ctx.Users.Add(user);
        await ctx.SaveChangesAsync(ct);

        // 5️⃣ Povrat odgovora
        return new RegisterCommandDto
        {
            UserId = user.Id,
            Email = user.Email,
            FullName = $"{user.FirstName} {user.LastName}",
            Message = "Uspešno ste registrovani. Sada se možete prijaviti."
        };
    }
}