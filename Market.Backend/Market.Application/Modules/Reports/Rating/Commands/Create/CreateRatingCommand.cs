using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;

namespace Market.Application.Modules.Reports.Rating.Commands.Create;

public sealed class CreateRatingCommand : IRequest<int>
{
    public required int UserId { get; init; }
    public required int ReportId { get; init; }
    public required int Rating { get; init; }
    public string? RatingComment { get; init; }
}

