using System.Data.Common;
using CleanTeeth.IntegrationTests.Services;
using CleanTeeth.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;

using Respawn;
using Security.Models;


namespace CleanTeeth.IntegrationTests.Fixtures;


public abstract class BaseIntegrationTest 

{
    protected IServiceScope _scope;
    protected CleanTeethDbContext _dbContext;
    protected TestWebApplicationFactory _factory;
    protected CancellationToken _token;
    protected HttpClient _client;
    protected string _connectionString;
    protected UserManager<User> _userManager;
    
    protected BaseIntegrationTest(TestContainersFixture fixture)
    {
        _connectionString = fixture.SqlConnectionString;
        _factory = new TestWebApplicationFactory(_connectionString);
        _scope = _factory.Services.CreateScope();
        _token = TestContext.Current.CancellationToken;
        
    }
    
   

    protected async Task SeedDb()
    {
        await _factory.EnsureDatabaseCreated(_scope, _token);
        _dbContext = _scope.ServiceProvider
            .GetRequiredService<CleanTeethDbContext>();
        _userManager = _scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        _client = _factory.CreateClient();
       
    }

    protected async Task ResetDb()
    {
        var stringBuilder = new SqlConnectionStringBuilder(_connectionString);
        stringBuilder.InitialCatalog = "test";
        var connection = new SqlConnection(stringBuilder.ConnectionString);
        
        await connection.OpenAsync();
        var respawner = await Respawner.CreateAsync(
            connection,
            new RespawnerOptions { SchemasToInclude = ["dbo"]}
        );
        await respawner.ResetAsync(connection);
        connection.Dispose();
    }

    protected async Task DisposeAll()
    {
        _scope.Dispose();
        await ResetDb();   
        await _dbContext.DisposeAsync();
        await _factory.DisposeAsync();
    }

    protected void Login(string userName,AppRoles role)
    {
        _client.DefaultRequestHeaders.Add(TestAuthHandler.TestUserIdHeader, userName);
        _client.DefaultRequestHeaders.Add(TestAuthHandler.TestUserRoleHeader, role.ToString());
    }
    
}