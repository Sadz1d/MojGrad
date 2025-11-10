using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Domain.Entities.Civic;
using Market.Application.Common.Exceptions;

namespace Market.Application.Modules.Civic.WatchList.Commands.Create
{
    public sealed class CreateWatchListCommandHandler
        : IRequestHandler<CreateWatchListCommand, int>
    {
        private readonly IAppDbContext _ctx;
        public CreateWatchListCommandHandler(IAppDbContext ctx) => _ctx = ctx;

        public async Task<int> Handle(CreateWatchListCommand request, CancellationToken ct)
        {
            // Opcionalno: spriječi duplikat (isti user + ista kategorija)
            var exists = await _ctx.WatchLists
                .AnyAsync(w => w.UserId == request.UserId && w.CategoryId == request.CategoryId, ct);

            if (exists)
                throw new MarketConflictException("This category is already in the user's watchlist.");

            var entity = new WatchListEntity
            {
                UserId = request.UserId,
                CategoryId = request.CategoryId,
                DateAdded = request.DateAdded ?? DateTime.UtcNow
            };

            _ctx.WatchLists.Add(entity);
            await _ctx.SaveChangesAsync(ct);

            return entity.Id;
        }
    }
}
