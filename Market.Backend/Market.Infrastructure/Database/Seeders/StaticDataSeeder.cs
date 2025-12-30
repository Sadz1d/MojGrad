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
using Microsoft.EntityFrameworkCore;

namespace Market.Infrastructure.Database.Seeders;

/// <summary>
/// Static seeder – koristi se u migracijama.
/// MojGrad trenutno nema static podatke.
/// </summary>
public static class StaticDataSeeder
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        // Nema static seed podataka za MojGrad
        // (korisnici i auth se seedaju dinamički)
    }
}
