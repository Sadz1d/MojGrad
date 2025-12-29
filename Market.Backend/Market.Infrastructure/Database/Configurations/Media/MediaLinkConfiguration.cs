using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Market.Domain.Entities.Media;

public class MediaLinkConfiguration
    : IEntityTypeConfiguration<MediaLinkEntity>
{
    public void Configure(EntityTypeBuilder<MediaLinkEntity> builder)
    {
        builder.ToTable("MediaLinks");

        builder.Property(x => x.EntityType)
            .IsRequired()
            .HasMaxLength(MediaLinkEntity.Constraints.EntityTypeMaxLength);

        builder.HasOne(x => x.Media)
            .WithMany()
            .HasForeignKey(x => x.MediaId);
    }
}
