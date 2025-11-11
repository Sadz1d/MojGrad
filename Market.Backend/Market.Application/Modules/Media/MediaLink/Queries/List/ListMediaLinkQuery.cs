using Market.Application.Modules.Media.MediaLink.Queries.List;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Market.Application.Modules.Media.MediaLink.Queries.List;

public sealed class ListMediaLinkQuery
    : BasePagedQuery<ListMediaLinkQueryDto>
{
    public string? EntityType { get; init; }   // npr. "ProblemReport", "ProofOfResolution"
    public int? EntityId { get; init; }
    public int? MediaId { get; init; } // ID entiteta kojem je medija vezana
}

