using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Market.Application.Modules.Reports.ProblemCategory.Queries.GetById;

public sealed class GetProblemCategoryByIdQuery : IRequest<GetProblemCategoryByIdQueryDto>
{
    public int Id { get; init; }
}

