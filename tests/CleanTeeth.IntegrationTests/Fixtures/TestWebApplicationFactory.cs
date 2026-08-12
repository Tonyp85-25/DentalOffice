
using CleanTeeth.IntegrationTests.Services;
using CleanTeeth.Persistence;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Security;
using Security.Models;


namespace CleanTeeth.IntegrationTests.Fixtures;

public class TestWebApplicationFactory: WebApplicationFactory<Program>
{
    private readonly string _dbConnectionString;

    public TestWebApplicationFactory(string dbConnectionString)
    {
        _dbConnectionString = dbConnectionString;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("testing");
        builder.UseSetting("ConnectionStrings:CleanTeethConnectionString", _dbConnectionString);
        builder.ConfigureAppConfiguration((_, config) =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>(StringComparer.Ordinal)
            {
                ["ConnectionStrings:CleanTeethConnectionString"] = _dbConnectionString
            });
        });
        builder.ConfigureServices( services =>
        {
           var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<CleanTeethDbContext>));

           if(descriptor != null)
           {
               services.Remove(descriptor);
           }
           var stringBuilder = new SqlConnectionStringBuilder(_dbConnectionString)
           {
               InitialCatalog = "test"
           };
           services.AddDbContext<CleanTeethDbContext>(options =>
           {
               options.UseSqlServer(stringBuilder.ConnectionString);
           });

           services.AddAuthentication(TestAuthHandler.AuthenticationScheme)
               .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                   TestAuthHandler.AuthenticationScheme, _ => { });

           // Override the default authentication scheme
           services.Configure<AuthenticationOptions>(options =>
           {
               options.DefaultAuthenticateScheme = TestAuthHandler.AuthenticationScheme;
               options.DefaultChallengeScheme = TestAuthHandler.AuthenticationScheme;
           });
        });
    }

    public static async Task EnsureDatabaseCreated(IServiceScope scope, CancellationToken token)
    {
        var securityDbContext = scope.ServiceProvider.GetRequiredService<SecurityDbContext>();
        await securityDbContext.Database.EnsureCreatedAsync(token);
        await IdentitySeeder.SeedRolesAsync(scope.ServiceProvider);
        await UserSeeder.SeedUsers(scope.ServiceProvider);
        var cleanTeethDbContext = scope.ServiceProvider.GetRequiredService<CleanTeethDbContext>();
        await cleanTeethDbContext.Database.EnsureCreatedAsync(token);

    }
}
