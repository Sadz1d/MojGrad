using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Reports.ProofOfResolution.Queries.GetById;

public sealed class GetProofOfResolutionByIdQueryDto
{
    public required int Id { get; init; }
    public required int TaskId { get; init; }
    public required DateTime UploadDate { get; init; }

    // opcionalno:
    public string? TaskTitle { get; init; }
}

