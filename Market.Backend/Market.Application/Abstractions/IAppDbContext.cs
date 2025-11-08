namespace Market.Application.Abstractions;

// Application layer
public interface IAppDbContext
{
    
    DbSet<MarketUserEntity> Users { get; }
    DbSet<RefreshTokenEntity> RefreshTokens { get; }

    Task<int> SaveChangesAsync(CancellationToken ct);
}