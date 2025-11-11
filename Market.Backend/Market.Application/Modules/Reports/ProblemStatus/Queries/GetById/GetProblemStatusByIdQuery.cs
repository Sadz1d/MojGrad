using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Market.Application.Modules.Reports.ProblemStatus.Queries.GetById;

public sealed class GetProblemStatusByIdQuery : IRequest<GetProblemStatusByIdQueryDto>
{
    public int Id { get; init; }
}

