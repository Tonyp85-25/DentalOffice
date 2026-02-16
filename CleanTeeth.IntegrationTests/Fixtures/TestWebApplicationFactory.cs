using System.Data.Common;
using CleanTeeth.Persistence;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;


namespace CleanTeeth.IntegrationTests.Fixtures;

public class TestWebApplicationFactory: WebApplicationFactory<Program>
{
    private readonly string _dbConnectionString;

    public TestWebApplicationFactory( string dbConnectionString)
    {
        _dbConnectionString = dbConnectionString;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("testing");
        // base.ConfigureWebHost(builder);
        builder.UseSetting("ConnectionStrings:CleanTeethConnectionString", _dbConnectionString);
        builder.ConfigureAppConfiguration((context, config) =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:CleanTeethConnectionString"] = _dbConnectionString
            });
        });
    }
}