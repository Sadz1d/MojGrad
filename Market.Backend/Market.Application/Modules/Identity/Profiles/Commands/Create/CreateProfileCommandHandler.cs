using MediatR;
using Market.Application.Common.Exceptions;
using Market.Domain.Entities.Identity;

namespace Market.Application.Modules.Identity.Profiles.Commands.Create;

public sealed class CreateProfileCommandHandler : IRequestHandler<CreateProfileCommand, int>
{
    private readonly IAppDbContext _ctx;

    public CreateProfileCommandHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<int> Handle(CreateProfileCommand request, CancellationToken ct)
    {
        // Optional: provjera da li user već ima profil
        var exists = await _ctx.Profiles
            .AnyAsync(p => p.UserId == request.UserId, ct);

        if (exists)
            throw new MarketConflictException($"User with Id {request.UserId} already has a profile.");

        var entity = new ProfileEntity
        {
            UserId = request.UserId,
            Address = request.Address?.Trim(),
            Phone = request.Phone?.Trim(),
            ProfilePicture = request.ProfilePicture,
            BiographyText = request.BiographyText
        };

        _ctx.Profiles.Add(entity);
        await _ctx.SaveChangesAsync(ct);

        return entity.Id;
    }
}
