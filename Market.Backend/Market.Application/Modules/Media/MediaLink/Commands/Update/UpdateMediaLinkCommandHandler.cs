using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Application.Common.Exceptions;
using Market.Domain.Entities.Media;

namespace Market.Application.Modules.Media.MediaLinks.Commands.Update;

public sealed class UpdateMediaLinkCommandHandler
    : IRequestHandler<UpdateMediaLinkCommand, Unit>
{
    private readonly IAppDbContext _ctx;
    public UpdateMediaLinkCommandHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<Unit> Handle(UpdateMediaLinkCommand request, CancellationToken ct)
    {
        var entity = await _ctx.MediaLinks
            .FirstOrDefaultAsync(x => x.Id == request.Id, ct);

        if (entity is null)
            throw new MarketNotFoundException($"MediaLink (Id={request.Id}) not found.");

        if (request.MediaId.HasValue)
        {
            var exists = await _ctx.MediaAttachments
                .AnyAsync(m => m.Id == request.MediaId.Value, ct);
            if (!exists)
                throw new MarketNotFoundException($"MediaAttachment (Id={request.MediaId.Value}) not found.");

            entity.MediaId = request.MediaId.Value;
        }

        if (!string.IsNullOrWhiteSpace(request.EntityType))
        {
            var type = request.EntityType.Trim();
            if (type.Length > MediaLinkEntity.Constraints.EntityTypeMaxLength)
                throw new ArgumentException(
                    $"EntityType max length is {MediaLinkEntity.Constraints.EntityTypeMaxLength}.");

            entity.EntityType = type;
        }

        if (request.EntityId.HasValue)
            entity.EntityId = request.EntityId.Value;

        // provjera duplikata nakon izmjena
        var dup = await _ctx.MediaLinks.AnyAsync(x =>
            x.Id != entity.Id &&
            x.MediaId == entity.MediaId &&
            x.EntityType == entity.EntityType &&
            x.EntityId == entity.EntityId, ct);

        if (dup)
            throw new MarketConflictException("A media link with the same Media/Entity already exists.");

        await _ctx.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
