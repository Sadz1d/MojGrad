using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Reports.ProblemReport.Queries.GetById;

public sealed class GetProblemReportByIdQueryDto
{
    public required int Id { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public string? Location { get; init; }
    public required DateTime CreationDate { get; init; }

    public required int UserId { get; init; }
    public required string AuthorName { get; init; }
    public required int CategoryId { get; init; }
    public required string CategoryName { get; init; }
    public required int StatusId { get; init; }
    public required string StatusName { get; init; }

    public required int CommentsCount { get; init; }
    public required int TasksCount { get; init; }
    public required int RatingsCount { get; init; }
    public string? ImagePath { get; set; }
}

