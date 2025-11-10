using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Market.Application.Modules.Media.MediaLinks.Queries.List;

public sealed class ListMediaLinksQuery
    : BasePagedQuery<ListMediaLinksQueryDto>
{
    public string? EntityType { get; init; }   // npr. "ProblemReport", "ProofOfResolution"
    public int? EntityId { get; init; }        // ID entiteta kojem je medija vezana
}

