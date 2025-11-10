using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Media.MediaLinks.Queries.List;

public sealed class ListMediaLinksQueryDto
{
    public required int Id { get; init; }
    public required int MediaId { get; init; }
    public required string FileUrl { get; init; }
    public required string MimeType { get; init; }
    public required long SizeBytes { get; init; }
    public required string EntityType { get; init; }
    public required int EntityId { get; init; }
    public required DateTime CreatedAt { get; init; }
}
