using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;
using System.Text.Json.Serialization;

namespace Market.Application.Modules.Reports.ProblemReport.Commands.Update;

public sealed class UpdateProblemReportCommand : IRequest<Unit>
{
    [JsonIgnore] public int Id { get; set; }

    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Location { get; set; }
    public int? CategoryId { get; set; }
    public int? StatusId { get; set; }
}

