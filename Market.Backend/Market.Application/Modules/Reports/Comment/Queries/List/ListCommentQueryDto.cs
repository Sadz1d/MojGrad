using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Reports.Comment.Queries.List;

public sealed class ListCommentQueryDto
{
    public required int Id { get; init; }
    public required int ReportId { get; init; }
    public required int UserId { get; init; }
    public required string UserName { get; init; }
    public required string Text { get; init; }
    public string? ShortText { get; init; }
    public required DateTime PublicationDate { get; init; }
}

