
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
    private  CleanTeethDbContext _dbContext;
    private CancellationToken _token;

    public DentalOfficesTest(TestContainersFixture fixture)
    {
        _factory = new TestWebApplicationFactory(fixture.SqlConnectionString);
       _client = _factory.CreateClient();
       _token = TestContext.Current.CancellationToken;
    }

    [Fact]
    public async Task ShouldCreateDentalOffice()
    {
        var dentalOffice = new CreateDentalOfficeDTO
            { Name = "Test", Street = "street",Zipcode = "11111", City = "city", Number = "1",Days = new List<int>{2,4,8,16}};
        var content = JsonContent.Create(dentalOffice);
        var response = await _client.PostAsync($"{_baseroute}", content,_token );
        
        Assert.Equal(HttpStatusCode.Created,response.StatusCode);
    }
    
    [Fact]
    public async Task ShouldReadDentalOffice()
    {
        Guid id =Guid.NewGuid();
        var address = Address.Create("1", "street", "11111", "city");
         _dbContext.DentalOffices.Add(DentalOffice.Create("Office A", id,address,62 ));
        await _dbContext.SaveChangesAsync(_token);
        
        var d= _dbContext.DentalOffices.ToList();
        var office = await _client.GetFromJsonAsync<DentalOfficeDetailDTO>($"{_baseroute}/{id}",_token);
            
        Assert.Equal("Office A", office?.Name);
        Assert.Equal("street", office.Street);
        Assert.Equal("11111", office.Zipcode);
        Assert.Equal("city", office.City);
        Assert.Equal("1", office.Number);
    }
    

    public async ValueTask DisposeAsync()
    {
        await _dbContext.DisposeAsync();
        await _factory.DisposeAsync();
       

    }

    public async ValueTask InitializeAsync()
    {
        // await using AsyncServiceScope scope = _factory.Services.CreateAsyncScope();
       _dbContext = _factory.Services.GetService<CleanTeethDbContext>()!;
        _dbContext?.Database.EnsureCreated();
      
    }
}