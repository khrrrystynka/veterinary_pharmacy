using VetPharmacyApi.Models;

namespace VetPharmacyApi.Data;

public static class DbSeeder
{
    public static void Seed(VetPharmacyDbContext context)
    {
        if (!context.Categories.Any())
        {
            var categories = new List<Category>
            {
                new() { Name = "Антибіотики" },
                new() { Name = "Протипаразитарні" },
                new() { Name = "Вітаміни" },
                new() { Name = "Засоби для догляду" },
                new() { Name = "Вакцини" }
            };
            context.Categories.AddRange(categories);
            context.SaveChanges();
        }

        if (!context.Products.Any())
        {
            var categories = context.Categories.ToList();
            var products = new List<Product>
            {
                new() {
                    Name = "Амоксицилін 15%",
                    Quantity = 100,
                    ArrivalDate = DateTime.Now.AddDays(-10),
                    ExpiryDate = DateTime.Now.AddYears(1),
                    IsWriteOffAllowed = true,
                    CategoryId = categories.First(c => c.Name == "Антибіотики").Id
                },
                new() {
                    Name = "Івермектин для собак",
                    Quantity = 50,
                    ArrivalDate = DateTime.Now.AddDays(-5),
                    ExpiryDate = DateTime.Now.AddYears(2),
                    IsWriteOffAllowed = true,
                    CategoryId = categories.First(c => c.Name == "Протипаразитарні").Id
                },
                new() {
                    Name = "Вітамікс B-комплекс",
                    Quantity = 30,
                    ArrivalDate = DateTime.Now.AddDays(-20),
                    ExpiryDate = DateTime.Now.AddMonths(6),
                    IsWriteOffAllowed = true,
                    CategoryId = categories.First(c => c.Name == "Вітаміни").Id
                },
                new() {
                    Name = "Шампунь для котів",
                    Quantity = 20,
                    ArrivalDate = DateTime.Now.AddDays(-2),
                    ExpiryDate = DateTime.Now.AddYears(1),
                    IsWriteOffAllowed = false,
                    CategoryId = categories.First(c => c.Name == "Засоби для догляду").Id
                },
                new() {
                    Name = "Вакцина Nobivac DHPPi",
                    Quantity = 15,
                    ArrivalDate = DateTime.Now.AddDays(-30),
                    ExpiryDate = DateTime.Now.AddMonths(4),
                    IsWriteOffAllowed = false,
                    CategoryId = categories.First(c => c.Name == "Вакцини").Id
                }
            };
            context.Products.AddRange(products);
            context.SaveChanges();
        }
    }
}

 