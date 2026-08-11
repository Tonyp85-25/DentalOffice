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
    protected IServiceScope Scope;
    protected CleanTeethDbContext DbContext;
    protected TestWebApplicationFactory Factory;
    protected CancellationToken Token;
    protected HttpClient Client;
    protected string ConnectionString;
    protected UserManager<User> UserManager;

    protected BaseIntegrationTest(TestContainersFixture fixture)
    {
        ConnectionString = fixture.SqlConnectionString;
        Factory = new TestWebApplicationFactory(ConnectionString);
        Scope = Factory.Services.CreateScope();
        Token = TestContext.Current.CancellationToken;

    }



    protected async Task SeedDb()
    {
        await Factory.EnsureDatabaseCreated(Scope, Token);
        DbContext = Scope.ServiceProvider
            .GetRequiredService<CleanTeethDbContext>();
        UserManager = Scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        Client = Factory.CreateClient();

    }

    protected async Task ResetDb()
    {
        var stringBuilder = new SqlConnectionStringBuilder(ConnectionString);
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
        Scope.Dispose();
        await ResetDb();
        await DbContext.DisposeAsync();
        await Factory.DisposeAsync();
    }

    protected void Login(string userName,AppRoles role)
    {
        Client.DefaultRequestHeaders.Add(TestAuthHandler.TestUserIdHeader, userName);
        Client.DefaultRequestHeaders.Add(TestAuthHandler.TestUserRoleHeader, role.ToString());
    }

}
