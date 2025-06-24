using Bogus;
using Microsoft.EntityFrameworkCore;
using Web.Api.Database;
using Web.Api.Entities;

namespace Web.Api.Extensions;

public static class DatabaseExtensions
{
    public static async Task ApplyMigrationsAsync(this WebApplication app)
    {
        using IServiceScope scope = app.Services.CreateScope();
        await using ApplicationDbContext applicationDbContext =
            scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        try
        {
            await applicationDbContext.Database.MigrateAsync();
            app.Logger.LogInformation("Application database migrations applied successfully.");
        }
        catch (Exception e)
        {
            app.Logger.LogError(e, "An error occurred while applying database migrations.");
            throw;
        }
    }

    public static async Task SeedInitialDataAsync(this WebApplication app)
    {
        using IServiceScope scope = app.Services.CreateScope();
        await using ApplicationDbContext applicationDbContext =
            scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        try
        {
            if (!await applicationDbContext.Customers.AnyAsync())
            {

                Faker<Customer> faker = new Faker<Customer>()
                    .RuleFor(p => p.Id, s => Customer.NewId())
                    .RuleFor(p => p.FirstName, s => s.Name.FirstName())
                    .RuleFor(p => p.LastName, s => s.Name.LastName())
                    .RuleFor(p => p.PhoneNumber, s => s.Phone.PhoneNumber("####-####"))
                    .RuleFor(p => p.IdentificationNumber, s => s.Random.Replace("########-#"))
                    .RuleFor(p => p.Email, s => s.Person.Email)
                    .RuleFor(p => p.Status, s => s.PickRandom<CustomerStatus>())
                    .RuleFor(p => p.BirthDate, s => s.Date.PastDateOnly());

                List<Customer> customers = faker.Generate(500);

                await applicationDbContext.AddRangeAsync(customers);

                await applicationDbContext.SaveChangesAsync();
            }

            app.Logger.LogInformation("Application database seeds applied successfully.");
        }
        catch (Exception e)
        {
            app.Logger.LogError(e, "An error occurred while applying database seeds.");
            throw;
        }
    }
}
