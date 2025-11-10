using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Media.Queries.List;

public sealed class ListMediaAttachmentsQueryDto
{
    public required int Id { get; init; }
    public required string FileUrl { get; init; }
    public required string MimeType { get; init; }
    public required long SizeBytes { get; init; }
    public required DateTime CreatedAt { get; init; }
    public required string UploaderName { get; init; }
}

