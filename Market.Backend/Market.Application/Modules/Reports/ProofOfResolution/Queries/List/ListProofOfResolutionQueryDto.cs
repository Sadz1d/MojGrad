using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Reports.ProofOfResolution.Queries.List;

public sealed class ListProofOfResolutionQueryDto
{
    public required int Id { get; init; }
    public required int TaskId { get; init; }
    public required DateTime UploadDate { get; init; }

}

