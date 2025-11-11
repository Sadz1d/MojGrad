using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Media.MediaLink.Queries.GetById;

public sealed class GetMediaLinkByIdQueryDto
{
    public required int Id { get; init; }
    public required int MediaId { get; init; }
    public required string EntityType { get; init; }
    public required int EntityId { get; init; }

    // podaci iz MediaAttachment-a
    public required string FileUrl { get; init; }
    public required string MimeType { get; init; }
    public required long SizeBytes { get; init; }
    public required DateTime CreatedAt { get; init; }
}

