using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Domain.Entities.Identity;
using Market.Application.Common.Exceptions;

namespace Market.Application.Modules.Identity.Users.Commands.Create;

public sealed class CreateMarketUserCommandHandler
    : IRequestHandler<CreateMarketUserCommand, int>
{
    private readonly IAppDbContext _ctx;

    public CreateMarketUserCommandHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<int> Handle(CreateMarketUserCommand request, CancellationToken ct)
    {
        // Provjeri postoji li već korisnik s istim emailom
        var exists = await _ctx.Users.AnyAsync(u => u.Email == request.Email, ct);
        if (exists)
            throw new MarketConflictException($"User with email {request.Email} already exists.");

        var user = new MarketUserEntity
        {
            FirstName = request.FirstName.Trim(),
            LastName = request.LastName.Trim(),
            Email = request.Email.Trim(),
            PasswordHash = request.PasswordHash,
            IsAdmin = request.IsAdmin,
            IsManager = request.IsManager,
            IsEmployee = request.IsEmployee,
            IsEnabled = request.IsEnabled,
            RegistrationDate = DateTime.UtcNow,
            Points = 0
        };

        _ctx.Users.Add(user);
        await _ctx.SaveChangesAsync(ct);

        return user.Id;
    }
}
