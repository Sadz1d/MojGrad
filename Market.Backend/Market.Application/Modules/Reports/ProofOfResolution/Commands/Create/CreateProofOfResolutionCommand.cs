using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;

namespace Market.Application.Modules.Reports.ProofOfResolution.Commands.Create;

public sealed class CreateProofOfResolutionCommand : IRequest<int>
{
    public required int TaskId { get; init; }
    public DateTime? UploadDate { get; init; } // ako je null, stavljamo UtcNow
}

