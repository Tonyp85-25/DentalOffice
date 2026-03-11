using CleanTeeth.Persistence;

namespace CleanTeeth.IntegrationTests.Fixtures;

public abstract class BaseIntegrationTest :  IClassFixture<TestContainersFixture>,
    IDisposable
{
    private readonly IServiceScope _scope;
    protected readonly CleanTeethDbContext DbContext;
    protected readonly TestWebApplicationFactory _factory;
    protected CancellationToken _token;

    protected BaseIntegrationTest(TestContainersFixture fixture)
    {
        _factory = new TestWebApplicationFactory(fixture.SqlConnectionString);
        _scope = _factory.Services.CreateScope();
        _token = TestContext.Current.CancellationToken;
        DbContext = _scope.ServiceProvider
            .GetRequiredService<CleanTeethDbContext>();
        DbContext.Database.EnsureCreated();
    }
    

    public void Dispose()
    {
        _factory.Dispose();
        _scope?.Dispose();
        DbContext?.Dispose();
    }
}