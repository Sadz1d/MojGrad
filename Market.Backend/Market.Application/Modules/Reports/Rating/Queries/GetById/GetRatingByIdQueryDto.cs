using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Reports.Rating.Queries.GetById;

public sealed class GetRatingByIdQueryDto
{
    public required int Id { get; init; }
    public required int UserId { get; init; }
    public required int ReportId { get; init; }
    public required int Rating { get; init; }
    public string? RatingComment { get; init; }
    public required string UserName { get; init; }
}

