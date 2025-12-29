//namespace Market.Infrastructure.Database.Seeders;

//public partial class StaticDataSeeder
//{
//    private static DateTime DateTime { get; set; } = new DateTime(2022, 4, 13, 1, 22, 18, 866, DateTimeKind.Local);

//    public static void Seed(ModelBuilder modelBuilder)
//    {
//        // Static data is added in the migration
//        // if it does not exist in the DB at the time of creating the migration
//        // example of static data: roles
//        SeedProductCategories(modelBuilder);
//    }

//    private static void SeedProductCategories(ModelBuilder modelBuilder)
//    {
//        // todo: user roles

//        //modelBuilder.Entity<UserRoles>().HasData(new List<UserRoleEntity>
//        //{
//        //    new UserRoleEntity{
//        //        Id = 1,
//        //        Name = "Admin",
//        //        CreatedAt = dateTime,
//        //        ModifiedAt = null,
//        //    },
//        //    new UserRoleEntity{
//        //        Id = 2,
//        //        Name = "Employee",
//        //        CreatedAt = dateTime,
//        //        ModifiedAt = null,
//        //    },
//        //});
//    }
//}
using Market.Domain.Entities.Reports;

namespace Market.Infrastructure.Database.Seeders;

public static class StaticDataSeeder
{
    private static readonly DateTime SeedDate =
        new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    public static void Seed(ModelBuilder modelBuilder)
    {
        SeedProblemStatuses(modelBuilder);
        SeedProblemCategories(modelBuilder);
    }

    private static void SeedProblemStatuses(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProblemStatusEntity>().HasData(
            new ProblemStatusEntity
            {
                Id = 1,
                Name = "Prijavljeno",
                CreatedAtUtc = SeedDate,
                IsDeleted = false
            },
            new ProblemStatusEntity
            {
                Id = 2,
                Name = "U toku",
                CreatedAtUtc = SeedDate,
                IsDeleted = false
            },
            new ProblemStatusEntity
            {
                Id = 3,
                Name = "Riješeno",
                CreatedAtUtc = SeedDate,
                IsDeleted = false
            }
        );
    }

    private static void SeedProblemCategories(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProblemCategoryEntity>().HasData(
            new ProblemCategoryEntity
            {
                Id = 1,
                Name = "Saobraćaj",
                Description = "Problemi na cestama",
                CreatedAtUtc = SeedDate,
                IsDeleted = false
            },
            new ProblemCategoryEntity
            {
                Id = 2,
                Name = "Otpad",
                Description = "Smeće i kontejneri",
                CreatedAtUtc = SeedDate,
                IsDeleted = false
            },
            new ProblemCategoryEntity
            {
                Id = 3,
                Name = "Rasvjeta",
                Description = "Ulična rasvjeta",
                CreatedAtUtc = SeedDate,
                IsDeleted = false
            }
        );
    }
}
