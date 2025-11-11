using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;
using System.Text.Json.Serialization;

namespace Market.Application.Modules.Report.Ratings.Commands.Update;

public sealed class UpdateRatingCommand : IRequest<Unit>
{
    [JsonIgnore] public int Id { get; set; }
    public int? Rating { get; set; }
    public string? RatingComment { get; set; }
}

