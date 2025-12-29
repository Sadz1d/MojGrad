public class ProfileConfiguration
    : IEntityTypeConfiguration<ProfileEntity>
{
    public void Configure(EntityTypeBuilder<ProfileEntity> builder)
    {
        builder.ToTable("Profiles");

        builder.HasOne(x => x.User)
            .WithOne(u => u.Profile)
            .HasForeignKey<ProfileEntity>(x => x.UserId);
    }
}
