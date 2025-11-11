using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;

namespace Market.Application.Modules.Reports.ProblemCategory.Commands.Create;

public sealed class CreateProblemCategoryCommand : IRequest<int>
{
    public required string Name { get; init; }
    public string? Description { get; init; }
}

