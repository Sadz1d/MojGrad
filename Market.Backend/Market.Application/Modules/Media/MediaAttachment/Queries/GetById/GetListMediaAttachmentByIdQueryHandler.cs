using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Application.Common.Exceptions;

namespace Market.Application.Modules.Media.Queries.GetById;

public sealed class GetMediaAttachmentByIdQueryHandler
    : IRequestHandler<GetMediaAttachmentByIdQuery, GetMediaAttachmentByIdQueryDto>
{
    private readonly IAppDbContext _ctx;
    public GetMediaAttachmentByIdQueryHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<GetMediaAttachmentByIdQueryDto> Handle(
        GetMediaAttachmentByIdQuery request, CancellationToken ct)
    {
        var dto = await _ctx.MediaAttachments
            .AsNoTracking()
            .Include(m => m.Uploader)
            .Where(m => m.Id == request.Id)
            .Select(m => new GetMediaAttachmentByIdQueryDto
            {
                Id = m.Id,
                FileUrl = m.FileUrl,
                MimeType = m.MimeType,
                SizeBytes = m.SizeBytes,
                CreatedAt = m.CreatedAt,
                UploaderName = m.Uploader != null
                    ? (m.Uploader.FirstName + " " + m.Uploader.LastName).Trim()
                    : "Nepoznat korisnik"
            })
            .FirstOrDefaultAsync(ct);

        if (dto is null)
            throw new MarketNotFoundException($"MediaAttachment with Id {request.Id} not found.");

        return dto;
    }
}

