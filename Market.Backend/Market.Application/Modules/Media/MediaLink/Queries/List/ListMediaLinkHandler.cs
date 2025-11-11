using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Domain.Entities.Media;

using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Domain.Entities.Media;


namespace Market.Application.Modules.Media.MediaLink.Queries.List;

public sealed class ListMediaLinkQueryHandler
    : IRequestHandler<ListMediaLinkQuery, PageResult<ListMediaLinkQueryDto>>
{
    private readonly IAppDbContext _ctx;
    public ListMediaLinkQueryHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<PageResult<ListMediaLinkQueryDto>> Handle(
        ListMediaLinkQuery request, CancellationToken ct)
    {
        IQueryable<MediaLinkEntity> q = _ctx.MediaLinks
            .AsNoTracking()
            .Include(x => x.Media);

        if (!string.IsNullOrWhiteSpace(request.EntityType))
        {
            var t = request.EntityType.Trim().ToLower();
            q = q.Where(x => x.EntityType.ToLower() == t);
        }

        if (request.EntityId.HasValue)
            q = q.Where(x => x.EntityId == request.EntityId.Value);

        if (request.MediaId.HasValue)
            q = q.Where(x => x.MediaId == request.MediaId.Value);

        var projected = q
            .OrderByDescending(x => x.Media.CreatedAt)
            .Select(x => new ListMediaLinkQueryDto
            {
                Id = x.Id,
                MediaId = x.MediaId,
                EntityType = x.EntityType,
                EntityId = x.EntityId,
                FileUrl = x.Media.FileUrl,
                MimeType = x.Media.MimeType,
                SizeBytes = x.Media.SizeBytes,
                CreatedAt = x.Media.CreatedAt
            });

        return await PageResult<ListMediaLinkQueryDto>
            .FromQueryableAsync(projected, request.Paging, ct);
    }
}


