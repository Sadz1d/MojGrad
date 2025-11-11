using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Domain.Entities.Media;
using Market.Application.Common.Exceptions;
using Market.Application.Modules.Media.MediaLink.Commands.Create;

namespace Market.Application.Modules.Media.MediaLinks.Commands.Create;

public sealed class CreateMediaLinkCommandHandler
    : IRequestHandler<CreateMediaLinkCommand, int>
{
    private readonly IAppDbContext _ctx;

    public CreateMediaLinkCommandHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<int> Handle(CreateMediaLinkCommand request, CancellationToken ct)
    {
        // 1️⃣ Provjeri da li MediaAttachment postoji
        var mediaExists = await _ctx.MediaAttachments
            .AnyAsync(m => m.Id == request.MediaId, ct);

        if (!mediaExists)
            throw new MarketNotFoundException($"MediaAttachment (Id={request.MediaId}) not found.");

        // 2️⃣ Validiraj dužinu EntityType bez custom exceptiona
        var type = request.EntityType.Trim();
        if (type.Length > MediaLinkEntity.Constraints.EntityTypeMaxLength)
            throw new ArgumentException(
                $"EntityType max length is {MediaLinkEntity.Constraints.EntityTypeMaxLength}.");

        // 3️⃣ Spriječi duplikate
        var exists = await _ctx.MediaLinks.AnyAsync(x =>
            x.MediaId == request.MediaId &&
            x.EntityType == type &&
            x.EntityId == request.EntityId, ct);

        if (exists)
            throw new MarketConflictException(
                "Media link already exists for the given Media/Entity pair.");

        // 4️⃣ Kreiraj entitet
        var entity = new MediaLinkEntity
        {
            MediaId = request.MediaId,
            EntityType = type,
            EntityId = request.EntityId
        };

        _ctx.MediaLinks.Add(entity);
        await _ctx.SaveChangesAsync(ct);

        return entity.Id;
    }
}
