namespace CleanTeeth.Persistence.Services.Seeding;

public interface ISeeder
{
    Task SeedAsync(CleanTeethDbContext context, IServiceProvider services);
}