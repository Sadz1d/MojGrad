using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Market.Domain.Entities.Events;

namespace Market.Infrastructure.Database.Configurations.Events;

public class EventCalendarConfiguration
    : IEntityTypeConfiguration<EventCalendarEntity>
{
    public void Configure(EntityTypeBuilder<EventCalendarEntity> builder)
    {
        builder.ToTable("EventsCalendar");

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(EventCalendarEntity.Constraints.NameMaxLength);

        builder.Property(x => x.Description)
            .HasMaxLength(EventCalendarEntity.Constraints.DescriptionMaxLength);

        builder.Property(x => x.EventDate)
            .IsRequired();

        builder.Property(x => x.EventType)
            .HasMaxLength(EventCalendarEntity.Constraints.TypeMaxLength);
    }
}