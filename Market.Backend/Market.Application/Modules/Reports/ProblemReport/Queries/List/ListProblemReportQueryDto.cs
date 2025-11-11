using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Reports.ProblemReport.Queries.List;

public sealed class ListProblemReportQueryDto
{
    public required int Id { get; init; }
    public required string Title { get; init; }
    public required string AuthorName { get; init; }
    public required DateTime CreationDate { get; init; }
    public string? Location { get; init; }
    public required string CategoryName { get; init; }
    public required string StatusName { get; init; }
    public required int CommentsCount { get; init; }
    public required int TasksCount { get; init; }
    public required int RatingsCount { get; init; }
    public string? ShortDescription { get; init; }
}
