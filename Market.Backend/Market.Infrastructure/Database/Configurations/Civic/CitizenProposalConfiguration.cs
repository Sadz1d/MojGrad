using Market.Domain.Entities.Civic;

public class CitizenProposalConfiguration
    : IEntityTypeConfiguration<CitizenProposalEntity>
{
    public void Configure(EntityTypeBuilder<CitizenProposalEntity> builder)
    {
        builder.ToTable("CitizenProposals");

        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(CitizenProposalEntity.Constraints.TitleMaxLength);

        builder.Property(x => x.Text)
            .IsRequired()
            .HasMaxLength(CitizenProposalEntity.Constraints.TextMaxLength);

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId);
    }
}
