using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Application.Common.Exceptions;
using Market.Domain.Entities.Civic;

namespace Market.Application.Modules.Civic.WatchList.Commands.Update
{
    public sealed class UpdateWatchListCommandHandler
        : IRequestHandler<UpdateWatchListCommand, Unit>
    {
        private readonly IAppDbContext _ctx;
        public UpdateWatchListCommandHandler(IAppDbContext ctx) => _ctx = ctx;

        public async Task<Unit> Handle(UpdateWatchListCommand request, CancellationToken ct)
        {
            var entity = await _ctx.WatchLists
                .FirstOrDefaultAsync(w => w.Id == request.Id, ct);

            if (entity is null)
                throw new MarketNotFoundException($"WatchList item (Id={request.Id}) not found.");

            // Ažuriranje polja samo ako su poslani
            if (request.CategoryId.HasValue)
                entity.CategoryId = request.CategoryId.Value;

            if (request.DateAdded.HasValue)
                entity.DateAdded = request.DateAdded.Value;

            await _ctx.SaveChangesAsync(ct);
            return Unit.Value;
        }
    }
}
