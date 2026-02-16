using DotNet.Testcontainers.Builders;
using Testcontainers.MsSql;

namespace CleanTeeth.IntegrationTests.Fixtures;

public sealed class TestContainersFixture :IAsyncLifetime
{
    private MsSqlContainer? _sqlContainer;
    public string SqlConnectionString { get; private set; } = string.Empty;
    
    public async ValueTask InitializeAsync()
    {
        
        _sqlContainer = new MsSqlBuilder("mcr.microsoft.com/mssql/server:2022-latest")
            .Build();
        await _sqlContainer.StartAsync();
        SqlConnectionString = _sqlContainer.GetConnectionString();
      
    }

    public async ValueTask DisposeAsync()
    {
        if (_sqlContainer != null)
        {
            await _sqlContainer.DisposeAsync();
        }
    }
}