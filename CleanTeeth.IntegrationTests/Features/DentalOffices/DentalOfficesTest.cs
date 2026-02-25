
using System.Net;
using CleanTeeth.API.DTO.DentalOffices;
using CleanTeeth.Application.Features.DentalOffices.Queries;
using CleanTeeth.Domain.Entities;
using CleanTeeth.Domain.ValueObjects;
using CleanTeeth.IntegrationTests.Fixtures;
using CleanTeeth.Persistence;

namespace CleanTeeth.IntegrationTests.Features.DentalOffices;

[Collection(nameof(IntegrationTestCollection))]
public sealed class DentalOfficesTest : IClassFixture<TestContainersFixture>, IAsyncLifetime
{
    private readonly TestWebApplicationFactory _factory;
    private readonly HttpClient _client;
    private string _baseroute = "api/dentaloffices";
    private CleanTeethDbContext _dbContext;

    public DentalOfficesTest(TestContainersFixture fixture)
    {
        _factory = new TestWebApplicationFactory(fixture.SqlConnectionString);
       _client = _factory.CreateClient();
    }

    [Fact]
    public async Task ShouldCreateDentalOffice()
    {
        var dentalOffice = new CreateDentalOfficeDTO
            { Name = "Test", Street = "street",Zipcode = "11111", City = "city", Number = "1"};
        var content = JsonContent.Create(dentalOffice);
        var response = await _client.PostAsync($"{_baseroute}", content);
        
        Assert.Equal(HttpStatusCode.Created,response.StatusCode);
    }
    
    [Fact]
    public async Task ShouldReadDentalOffice()
    {
        Guid id =Guid.NewGuid();
        var address = Address.Create("1", "street", "11111", "city");
         _dbContext.DentalOffices.Add(DentalOffice.Create("Office A", id,address));
        await _dbContext.SaveChangesAsync();
        
        var d= _dbContext.DentalOffices.ToList();
        var office = await _client.GetFromJsonAsync<DentalOfficeDetailDTO>($"{_baseroute}/{id}");
            
        Assert.Equal("Office A", office.Name);
        Assert.Equal("street", office.Street);
        Assert.Equal("11111", office.Zipcode);
        Assert.Equal("city", office.City);
        Assert.Equal("1", office.Number);
    }
    

    public async ValueTask DisposeAsync()
    {
        await _factory.DisposeAsync();
        await _dbContext.DisposeAsync();

    }

    public async ValueTask InitializeAsync()
    {
        // await using AsyncServiceScope scope = _factory.Services.CreateAsyncScope();
       _dbContext = _factory.Services.GetService<CleanTeethDbContext>();
        _dbContext.Database.EnsureCreated();
      
    }
}