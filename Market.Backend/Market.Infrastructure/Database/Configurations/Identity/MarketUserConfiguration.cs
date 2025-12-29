using Market.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Market.Infrastructure.Database.Configurations.Identity;

public sealed class MarketUserConfiguration : IEntityTypeConfiguration<MarketUserEntity>
{
    public void Configure(EntityTypeBuilder<MarketUserEntity> b)
    {
        b.ToTable("Users");

        b.HasKey(x => x.Id);

        // ========================
        // Properties
        // ========================

        b.Property(x => x.FirstName)
            .HasMaxLength(MarketUserEntity.Constraints.NameMaxLength);

        b.Property(x => x.LastName)
            .HasMaxLength(MarketUserEntity.Constraints.NameMaxLength);

        b.Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(MarketUserEntity.Constraints.EmailMaxLength);

        b.HasIndex(x => x.Email)
            .IsUnique();

        b.Property(x => x.PasswordHash)
            .IsRequired()
            .HasMaxLength(MarketUserEntity.Constraints.PasswordHashMaxLength);

        b.Property(x => x.RegistrationDate)
            .IsRequired();

        b.Property(x => x.Points)
            .HasDefaultValue(0);

        // ========================
        // Flags / Status
        // ========================

        b.Property(x => x.IsAdmin)
            .HasDefaultValue(false);

        b.Property(x => x.IsManager)
            .HasDefaultValue(false);

        b.Property(x => x.IsEmployee)
            .HasDefaultValue(true);

        b.Property(x => x.IsEnabled)
            .HasDefaultValue(true);

        b.Property(x => x.TokenVersion)
            .HasDefaultValue(0);

        // ========================
        // Relationships
        // ========================

        b.HasOne(x => x.Profile)
            .WithOne(x => x.User)
            .HasForeignKey<ProfileEntity>(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        b.HasMany(x => x.RefreshTokens)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        b.HasMany(x => x.Reports)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId);

        b.HasMany(x => x.Comments)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId);

        b.HasMany(x => x.SurveyResponses)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId);

        b.HasMany(x => x.AssignedRewards)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId);

        b.HasMany(x => x.WatchList)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId);

        b.HasMany(x => x.TasksAsWorker)
            .WithOne(x => x.Worker)
            .HasForeignKey(x => x.WorkerId);

        b.HasMany(x => x.Ratings)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId);

        b.HasMany(x => x.ActionParticipations)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId);
    }
}
