using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Domain.Entities.Identity;
using Market.Application.Common.Exceptions;

namespace Market.Application.Modules.Identity.Users.Queries.GetById;

public sealed class GetMarketUserByIdQueryHandler : IRequestHandler<GetMarketUserByIdQuery, GetMarketUserByIdQueryDto>
{
    private readonly IAppDbContext _ctx;

    public GetMarketUserByIdQueryHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<GetMarketUserByIdQueryDto> Handle(GetMarketUserByIdQuery request, CancellationToken ct)
    {
        var user = await _ctx.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == request.Id, ct);

        if (user == null)
            throw new MarketNotFoundException($"User with Id {request.Id} not found.");

        return new GetMarketUserByIdQueryDto
        {
            Id = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            IsAdmin = user.IsAdmin,
            IsManager = user.IsManager,
            IsEmployee = user.IsEmployee,
            IsEnabled = user.IsEnabled,
            RegistrationDate = user.RegistrationDate
        };
    }
}
