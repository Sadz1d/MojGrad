using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Market.Application.Modules.Reports.ProofOfResolution.Queries.List;

namespace Market.Application.Modules.Reports.ProofsOfResolution.Queries.List;

public sealed class ListProofOfResolutionQuery
    : BasePagedQuery<ListProofOfResolutionQueryDto>
{
    public int? TaskId { get; init; }     // filtar po Tasku
    public DateTime? From { get; init; }  // od datuma
    public DateTime? To { get; init; }    // do datuma (inclusive)
}


