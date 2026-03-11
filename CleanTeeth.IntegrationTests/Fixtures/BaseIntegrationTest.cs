using System.Data.Common;
using CleanTeeth.Persistence;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using Respawn;

namespace CleanTeeth.IntegrationTests.Fixtures;

public abstract class BaseIntegrationTest :  IClassFixture<TestContainersFixture>,
    IAsyncDisposable
{
    private readonly IServiceScope _scope;
    protected readonly CleanTeethDbContext _dbContext;
    protected readonly TestWebApplicationFactory _factory;
    protected CancellationToken _token;
    protected readonly HttpClient _client;
    private readonly string _connectionString;

    protected BaseIntegrationTest(TestContainersFixture fixture)
    {
        _connectionString = fixture.SqlConnectionString;
        _factory = new TestWebApplicationFactory(fixture.SqlConnectionString);
        _client = _factory.CreateClient();
        _scope = _factory.Services.CreateScope();
        _token = TestContext.Current.CancellationToken;
        _dbContext = _scope.ServiceProvider
            .GetRequiredService<CleanTeethDbContext>();
        _dbContext.Database.EnsureCreated();
    }

    
    public async ValueTask DisposeAsync()
    {
        await ResetDb();   
        await _factory.DisposeAsync();
        _scope?.Dispose();
         _dbContext?.Dispose();
    }

    private async Task ResetDb()
    {
        var conn = new SqlConnection(_connectionString)  ;

        await conn.OpenAsync();
        var respawner = await Respawner.CreateAsync(
            conn,
            new RespawnerOptions { SchemasToInclude = ["dbo"]}
        );
        await respawner.ResetAsync(conn);
        conn.Dispose();
    }
    
}