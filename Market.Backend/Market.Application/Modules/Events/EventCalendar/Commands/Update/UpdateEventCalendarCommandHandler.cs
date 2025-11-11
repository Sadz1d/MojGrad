using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Application.Common.Exceptions;
using Market.Domain.Entities.Events;

namespace Market.Application.Modules.Events.EventCalendar.Commands.Update
{
    public sealed class UpdateEventCalendarCommandHandler
        : IRequestHandler<UpdateEventCalendarCommand, Unit>
    {
        private readonly IAppDbContext _ctx;

        public UpdateEventCalendarCommandHandler(IAppDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<Unit> Handle(UpdateEventCalendarCommand request, CancellationToken ct)
        {
            var entity = await _ctx.EventsCalendar
                .FirstOrDefaultAsync(e => e.Id == request.Id, ct);

            if (entity == null)
                throw new MarketNotFoundException($"Event with Id {request.Id} not found.");

            entity.Name = request.Name.Trim();
            entity.Description = request.Description?.Trim();
            entity.EventDate = request.EventDate;
            entity.EventType = request.EventType?.Trim();

            _ctx.EventsCalendar.Update(entity);
            await _ctx.SaveChangesAsync(ct);

            return Unit.Value;
        }
    }
}
