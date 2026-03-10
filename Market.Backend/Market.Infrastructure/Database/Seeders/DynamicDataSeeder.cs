//namespace Market.Infrastructure.Database.Seeders;

///// <summary>
///// Dynamic seeder koji se pokreće u runtime-u,
///// obično pri startu aplikacije (npr. u Program.cs).
///// Koristi se za unos demo/test podataka koji nisu dio migracije.
///// </summary>
//public static class DynamicDataSeeder
//{
//    public static async Task SeedAsync(DatabaseContext context)
//    {
//        // Osiguraj da baza postoji (bez migracija)
//        await context.Database.EnsureCreatedAsync();

//        //await SeedProductCategoriesAsync(context);
//        await SeedUsersAsync(context);
//    }

//    //private static async Task SeedProductCategoriesAsync(DatabaseContext context)
//    //{
//    //    if (!await context.ProductCategories.AnyAsync())
//    //    {
//    //        context.ProductCategories.AddRange(
//    //            new ProductCategoryEntity
//    //            {
//    //                Name = "Računari (demo)",
//    //                IsEnabled = true,
//    //                CreatedAtUtc = DateTime.UtcNow
//    //            },
//    //            new ProductCategoryEntity
//    //            {
//    //                Name = "Mobilni uređaji (demo)",
//    //                IsEnabled = true,
//    //                CreatedAtUtc = DateTime.UtcNow
//    //            }
//    //        );

//    //        await context.SaveChangesAsync();
//    //        Console.WriteLine("✅ Dynamic seed: product categories added.");
//    //    }
//    //}

//    /// <summary>
//    /// Kreira demo korisnike ako ih još nema u bazi.
//    /// </summary>
//    private static async Task SeedUsersAsync(DatabaseContext context)
//    {
//        if (await context.Users.AnyAsync())
//            return;

//        var hasher = new PasswordHasher<MarketUserEntity>();

//        var admin = new MarketUserEntity
//        {
//            Email = "admin@market.local",
//            PasswordHash = hasher.HashPassword(null!, "Admin123!"),
//            IsAdmin = true,
//            IsEnabled = true,
//        };

//        var user = new MarketUserEntity
//        {
//            Email = "manager@market.local",
//            PasswordHash = hasher.HashPassword(null!, "User123!"),
//            IsManager = true,
//            IsEnabled = true,
//        };

//        var dummyForSwagger = new MarketUserEntity
//        {
//            Email = "string",
//            PasswordHash = hasher.HashPassword(null!, "string"),
//            IsEmployee = true,
//            IsEnabled = true,
//        };
//        var dummyForTests = new MarketUserEntity
//        {
//            Email = "test",
//            PasswordHash = hasher.HashPassword(null!, "test123"),
//            IsEmployee = true,
//            IsEnabled = true,
//        };
//        context.Users.AddRange(admin, user, dummyForSwagger, dummyForTests);
//        await context.SaveChangesAsync();

//        Console.WriteLine("✅ Dynamic seed: demo users added.");
//    }
//}
using Market.Domain.Entities.Reports;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Market.Infrastructure.Database.Seeders;

public static class DynamicDataSeeder
{
    public static async Task SeedAsync(DatabaseContext context)
    {
        await SeedUsersAsync(context);
        await SeedProblemStatusesAsync(context);
        await SeedProblemCategoriesAsync(context);
    }

    public static async Task SeedUsersAsync(DatabaseContext context)
    {
        if (await context.Users.AnyAsync())
            return;

        var hasher = new PasswordHasher<MarketUserEntity>();

        var admin = new MarketUserEntity
        {
            Email = "admin@mojgrad.local",
            PasswordHash = hasher.HashPassword(null!, "Admin123!"),
            IsAdmin = true,
            IsEnabled = true,
            RegistrationDate = DateTime.UtcNow
        };

        var manager = new MarketUserEntity
        {
            Email = "manager@mojgrad.local",
            PasswordHash = hasher.HashPassword(null!, "Manager123!"),
            IsManager = true,
            IsEnabled = true,
            RegistrationDate = DateTime.UtcNow
        };

        var employee = new MarketUserEntity
        {
            Email = "employee@mojgrad.local",
            PasswordHash = hasher.HashPassword(null!, "Employee123!"),
            IsEmployee = true,
            IsEnabled = true,
            RegistrationDate = DateTime.UtcNow
        };

        context.Users.AddRange(admin, manager, employee);
        await context.SaveChangesAsync();
    }

    public static async Task SeedProblemStatusesAsync(DatabaseContext context)
    {
        if (await context.ProblemStatuses.AnyAsync())
            return;

        var statuses = new List<ProblemStatusEntity>
        {
            new() { Name = "Novo",     CreatedAtUtc = DateTime.UtcNow },
            new() { Name = "U toku",   CreatedAtUtc = DateTime.UtcNow },
            new() { Name = "Riješeno", CreatedAtUtc = DateTime.UtcNow },
            new() { Name = "Odbijeno", CreatedAtUtc = DateTime.UtcNow },
            new() { Name = "Na čekanju", CreatedAtUtc = DateTime.UtcNow },
        };

        context.ProblemStatuses.AddRange(statuses);
        await context.SaveChangesAsync();
        Console.WriteLine("✅ Seed: problem statusi dodani.");
    }

    public static async Task SeedProblemCategoriesAsync(DatabaseContext context)
    {
        if (await context.ProblemCategories.AnyAsync())
            return;

        var categories = new List<ProblemCategoryEntity>
        {
            new() { Name = "Ceste i saobraćaj",    Description = "Rupe, oštećenja, semafori",         IsEnabled = true, CreatedAtUtc = DateTime.UtcNow },
            new() { Name = "Javna rasvjeta",        Description = "Pokvarene lampe, mrežni kvarovi",   IsEnabled = true, CreatedAtUtc = DateTime.UtcNow },
            new() { Name = "Komunalni otpad",       Description = "Divlje deponije, pretrpane kante",  IsEnabled = true, CreatedAtUtc = DateTime.UtcNow },
            new() { Name = "Parkovi i zelene površine", Description = "Neodržani parkovi, stabla",    IsEnabled = true, CreatedAtUtc = DateTime.UtcNow },
            new() { Name = "Vodovod i kanalizacija",Description = "Kvarovi, curenja, začepljenja",     IsEnabled = true, CreatedAtUtc = DateTime.UtcNow },
            new() { Name = "Javni prijevoz",        Description = "Autobusi, stajališta, vozni red",   IsEnabled = true, CreatedAtUtc = DateTime.UtcNow },
            new() { Name = "Vandalizem",            Description = "Grafiti, oštećena imovina",         IsEnabled = true, CreatedAtUtc = DateTime.UtcNow },
            new() { Name = "Ostalo",                Description = "Sve ostalo što ne spada u gore",    IsEnabled = true, CreatedAtUtc = DateTime.UtcNow },
        };

        context.ProblemCategories.AddRange(categories);
        await context.SaveChangesAsync();
        Console.WriteLine("✅ Seed: kategorije problema dodane.");
    }
}