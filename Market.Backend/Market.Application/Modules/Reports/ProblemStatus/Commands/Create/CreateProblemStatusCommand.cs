using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;

namespace Market.Application.Modules.Reports.ProblemStatus.Commands.Create;

public sealed class CreateProblemStatusCommand : IRequest<int>
{
    public required string Name { get; init; }
}

