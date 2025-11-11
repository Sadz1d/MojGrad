using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Domain.Entities.Identity;

namespace Market.Application.Modules.Identity.Users.Queries.List;

public sealed class ListMarketUsersQueryHandler : IRequestHandler<ListMarketUsersQuery, PageResult<ListMarketUsersQueryDto>>
{
    private readonly IAppDbContext _ctx;

    public ListMarketUsersQueryHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<PageResult<ListMarketUsersQueryDto>> Handle(ListMarketUsersQuery request, CancellationToken ct)
    {
        IQueryable<MarketUserEntity> q = _ctx.Users.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var term = request.Search.Trim().ToLower();
            q = q.Where(u =>
                u.Email.ToLower().Contains(term) ||
                (u.FirstName != null && u.FirstName.ToLower().Contains(term)) ||
                (u.LastName != null && u.LastName.ToLower().Contains(term))
            );
        }

        if (request.OnlyEnabled.HasValue)
            q = q.Where(u => u.IsEnabled == request.OnlyEnabled.Value);

        var projected = q
            .OrderByDescending(u => u.RegistrationDate)
            .Select(u => new ListMarketUsersQueryDto
            {
                Id = u.Id,
                Email = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName,
                IsAdmin = u.IsAdmin,
                IsManager = u.IsManager,
                IsEmployee = u.IsEmployee,
                IsEnabled = u.IsEnabled,
                RegistrationDate = u.RegistrationDate
            });

        return await PageResult<ListMarketUsersQueryDto>.FromQueryableAsync(projected, request.Paging, ct);
    }
}
