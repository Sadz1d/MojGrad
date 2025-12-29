using Market.Domain.Entities.Civic;

public class WatchListConfiguration
    : IEntityTypeConfiguration<WatchListEntity>
{
    public void Configure(EntityTypeBuilder<WatchListEntity> builder)
    {
        builder.ToTable("WatchLists");

        builder.HasOne(x => x.User)
            .WithMany(u => u.WatchList)
            .HasForeignKey(x => x.UserId);

        builder.HasOne(x => x.Category)
            .WithMany()
            .HasForeignKey(x => x.CategoryId);
    }
}
