using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Media.Queries.GetById;

public sealed class GetMediaAttachmentByIdQueryDto
{
    public required int Id { get; init; }
    public required string FileUrl { get; init; }
    public required string MimeType { get; init; }
    public required long SizeBytes { get; init; }
    public required DateTime CreatedAt { get; init; }
    public required string UploaderName { get; init; } // Ime i prezime ili fallback
}

