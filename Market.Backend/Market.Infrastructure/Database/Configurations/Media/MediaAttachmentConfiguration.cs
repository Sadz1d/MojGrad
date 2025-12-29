using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Market.Domain.Entities.Media;

public class MediaAttachmentConfiguration
    : IEntityTypeConfiguration<MediaAttachmentEntity>
{
    public void Configure(EntityTypeBuilder<MediaAttachmentEntity> builder)
    {
        builder.ToTable("MediaAttachments");

        builder.Property(x => x.FileUrl)
            .IsRequired()
            .HasMaxLength(MediaAttachmentEntity.Constraints.UrlMaxLength);

        builder.Property(x => x.MimeType)
            .IsRequired()
            .HasMaxLength(MediaAttachmentEntity.Constraints.MimeMaxLength);

        builder.HasOne(x => x.Uploader)
            .WithMany()
            .HasForeignKey(x => x.UploaderId);
    }
}
