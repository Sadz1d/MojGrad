using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;
using System.Text.Json.Serialization;

namespace Market.Application.Modules.Reports.ProblemStatus.Commands.Update;

public sealed class UpdateProblemStatusCommand : IRequest<Unit>
{
    [JsonIgnore] public int Id { get; set; }
    public string? Name { get; set; }
}

