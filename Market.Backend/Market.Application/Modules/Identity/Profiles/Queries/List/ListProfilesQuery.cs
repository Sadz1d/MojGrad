using Market.Application.Modules.Identity.Profiles.Queries.List;
using Market.Domain.Common;
using MediatR;

namespace Market.Application.Modules.Identity.Profiles.Queries.List;

public sealed class ListProfilesQuery : BasePagedQuery<ListProfilesQueryDto>, IRequest<PageResult<ListProfilesQueryDto>>
{
    public string? Search { get; init; } // opcionalno pretraživanje po adresi, telefonu ili biografiji
}
