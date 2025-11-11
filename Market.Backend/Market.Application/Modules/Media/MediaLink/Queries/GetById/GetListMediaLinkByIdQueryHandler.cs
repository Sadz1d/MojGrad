using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Application.Common.Exceptions;

namespace Market.Application.Modules.Media.MediaLink.Queries.GetById;

public sealed class GetMediaLinkByIdQueryHandler
    : IRequestHandler<GetMediaLinkByIdQuery, GetMediaLinkByIdQueryDto>
{
    private readonly IAppDbContext _ctx;
    public GetMediaLinkByIdQueryHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<GetMediaLinkByIdQueryDto> Handle(
        GetMediaLinkByIdQuery request, CancellationToken ct)
    {
        var dto = await _ctx.MediaLinks
            .AsNoTracking()
            .Include(x => x.Media)
            .Where(x => x.Id == request.Id)
            .Select(x => new GetMediaLinkByIdQueryDto
            {
                Id = x.Id,
                MediaId = x.MediaId,
                EntityType = x.EntityType,
                EntityId = x.EntityId,
                FileUrl = x.Media.FileUrl,
                MimeType = x.Media.MimeType,
                SizeBytes = x.Media.SizeBytes,
                CreatedAt = x.Media.CreatedAt
            })
            .FirstOrDefaultAsync(ct);

        if (dto is null)
            throw new MarketNotFoundException($"MediaLink (Id={request.Id}) not found.");

        return dto;
    }
}

