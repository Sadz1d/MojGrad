using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Market.Application.Modules.Reports.ProofOfResolution.Queries.GetById;

public sealed class GetProofOfResolutionByIdQuery : IRequest<GetProofOfResolutionByIdQueryDto>
{
    public int Id { get; init; }
}

