using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Domain.Entities.Media;

namespace Market.Application.Modules.Media.Queries.List;

public sealed class ListMediaAttachmentsQueryHandler
    : IRequestHandler<ListMediaAttachmentsQuery, PageResult<ListMediaAttachmentsQueryDto>>
{
    private readonly IAppDbContext _ctx;

    public ListMediaAttachmentsQueryHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<PageResult<ListMediaAttachmentsQueryDto>> Handle(
        ListMediaAttachmentsQuery request, CancellationToken ct)
    {
        IQueryable<MediaAttachmentEntity> q = _ctx.MediaAttachments
            .AsNoTracking()
            .Include(m => m.Uploader);

        // Filtriranje po MIME tipu
        if (!string.IsNullOrWhiteSpace(request.SearchMime))
        {
            var term = request.SearchMime.Trim().ToLower();
            q = q.Where(m => m.MimeType.ToLower().Contains(term));
        }

        // Filtriranje po uploaderu
        if (request.UploaderId.HasValue)
            q = q.Where(m => m.UploaderId == request.UploaderId.Value);

        // Projekcija u DTO
        var projected = q
            .OrderByDescending(m => m.CreatedAt)
            .Select(m => new ListMediaAttachmentsQueryDto
            {
                Id = m.Id,
                FileUrl = m.FileUrl,
                MimeType = m.MimeType,
                SizeBytes = m.SizeBytes,
                CreatedAt = m.CreatedAt,
                UploaderName = m.Uploader != null
                    ? (m.Uploader.FirstName + " " + m.Uploader.LastName).Trim()
                    : "Nepoznat korisnik"
            });

        return await PageResult<ListMediaAttachmentsQueryDto>
            .FromQueryableAsync(projected, request.Paging, ct);
    }
}

