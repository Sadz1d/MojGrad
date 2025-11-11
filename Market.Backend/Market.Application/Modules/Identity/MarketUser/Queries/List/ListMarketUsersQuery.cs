using Market.Domain.Common;
using MediatR;

namespace Market.Application.Modules.Identity.Users.Queries.List;

public sealed class ListMarketUsersQuery : BasePagedQuery<ListMarketUsersQueryDto>, IRequest<PageResult<ListMarketUsersQueryDto>>
{
    public string? Search { get; init; }      // Pretraga po emailu ili imenu
    public bool? OnlyEnabled { get; init; }   // Samo aktivni korisnici
}
