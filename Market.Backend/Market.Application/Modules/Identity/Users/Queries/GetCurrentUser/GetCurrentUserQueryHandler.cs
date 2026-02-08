using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Application.Common.Exceptions;

namespace Market.Application.Modules.Identity.Users.Queries.GetCurrentUser;

public sealed class GetCurrentUserQueryHandler
    : IRequestHandler<GetCurrentUserQuery, GetCurrentUserQueryDto>
{
    private readonly IAppDbContext _ctx;

    public GetCurrentUserQueryHandler(IAppDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<GetCurrentUserQueryDto> Handle(
        GetCurrentUserQuery request,
        CancellationToken ct)
    {
        var user = await _ctx.Users
            .AsNoTracking()
            .Where(u => u.Id == request.UserId)
            .Select(u => new GetCurrentUserQueryDto
            {
                Id = u.Id,
                Email = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName,
                FullName = (u.FirstName ?? "") + " " + (u.LastName ?? ""),
                IsAdmin = u.IsAdmin,
                IsManager = u.IsManager,
                IsEmployee = u.IsEmployee
            })
            .FirstOrDefaultAsync(ct);

        if (user == null)
            throw new MarketNotFoundException(
                $"User with Id {request.UserId} not found.");

        return user;
    }
}
