using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;
using System.Text.Json.Serialization;

namespace Market.Application.Modules.Reports.ProblemCategory.Commands.Update;

public sealed class UpdateProblemCategoryCommand : IRequest<Unit>
{
    [JsonIgnore] public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
}

