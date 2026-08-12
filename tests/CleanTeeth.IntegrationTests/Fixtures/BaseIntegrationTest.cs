using System.Data.Common;
using CleanTeeth.IntegrationTests.Services;
using CleanTeeth.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;

using Respawn;
using Security.Models;


namespace CleanTeeth.IntegrationTests.Fixtures;


public abstract class BaseIntegrationTest:IAsyncLifetime

{
    internal  IServiceScope Scope;
    internal CleanTeethDbContext DbContext;
    internal TestWebApplicationFactory Factory;
    internal CancellationToken Token;
    internal HttpClient Client;
    internal string ConnectionString;
    internal UserManager<User> UserManager;

    protected BaseIntegrationTest(TestContainersFixture fixture)
    {
        ConnectionString = fixture.SqlConnectionString;
        Factory = new TestWebApplicationFactory(ConnectionString);
        Scope = Factory.Services.CreateScope();
        Token = TestContext.Current.CancellationToken;

    }



    protected async Task SeedDb()
    {
        await TestWebApplicationFactory.EnsureDatabaseCreated(Scope, Token);
        DbContext = Scope.ServiceProvider
            .GetRequiredService<CleanTeethDbContext>();
        UserManager = Scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        Client = Factory.CreateClient();

    }

    protected async Task ResetDb()
    {
        var stringBuilder = new SqlConnectionStringBuilder(ConnectionString)
        {
            InitialCatalog = "test"
        };
        var connection = new SqlConnection(stringBuilder.ConnectionString);

        await connection.OpenAsync(Token);
        var respawner = await Respawner.CreateAsync(
            connection,
            new RespawnerOptions { SchemasToInclude = ["dbo"]}
        );
        await respawner.ResetAsync(connection);
        await connection.DisposeAsync();
    }

   public async ValueTask DisposeAsync()
    {
        Scope.Dispose();
        await ResetDb();
        await DbContext.DisposeAsync();
        await Factory.DisposeAsync();
        Client.Dispose();
        UserManager.Dispose();
        GC.SuppressFinalize(this);
    }

    public virtual async ValueTask InitializeAsync()
    {
        await SeedDb();
    }

    protected void Login(string userName,AppRoles role)
    {
        Client.DefaultRequestHeaders.Add(TestAuthHandler.TestUserIdHeader, userName);
        Client.DefaultRequestHeaders.Add(TestAuthHandler.TestUserRoleHeader, role.ToString());
    }


}
