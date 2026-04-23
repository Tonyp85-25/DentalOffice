namespace CleanTeeth.Persistence.Services.Seeding;

public sealed class ApplicationSeeder
{
    private readonly IEnumerable<ISeeder> _seeders;
    public ApplicationSeeder(IEnumerable<ISeeder> seeders) => _seeders = seeders;

    public async Task SeedAllAsync(CleanTeethDbContext context, IServiceProvider services)
    {
        foreach (var seeder in _seeders)
        {
            await seeder.SeedAsync(context, services);
        }
    }
}