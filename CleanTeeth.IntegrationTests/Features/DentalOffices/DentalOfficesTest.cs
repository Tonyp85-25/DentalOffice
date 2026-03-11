
using System.Net;
using CleanTeeth.API.DTO.DentalOffices;
using CleanTeeth.Application.Features.DentalOffices.Queries;
using CleanTeeth.Fixtures.Entities;
using CleanTeeth.IntegrationTests.Fixtures;

namespace CleanTeeth.IntegrationTests.Features.DentalOffices;

[Collection(nameof(IntegrationTestCollection))]
public sealed class DentalOfficesTest : BaseIntegrationTest
{
    
    private string _baseroute = "api/dentaloffices";
    
    private readonly DentalOfficeBuilder _builder;

    public DentalOfficesTest(TestContainersFixture fixture) : base(fixture)
    {
       
       _builder = new DentalOfficeBuilder();
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
        var dentalOffice = _builder.WithGuid(id).WithName("Office A").WithAddress("1;street;11111;city").WithDays(62).Build();
         _dbContext.DentalOffices.Add(dentalOffice);
        await _dbContext.SaveChangesAsync(_token);
        
        var d= _dbContext.DentalOffices.ToList();
        var office = await _client.GetFromJsonAsync<DentalOfficeDetailDTO>($"{_baseroute}/{id}",_token);
            
        Assert.Equal("Office A", office?.Name);
        Assert.Equal("street", office.Street);
        Assert.Equal("11111", office.Zipcode);
        Assert.Equal("city", office.City);
        Assert.Equal("1", office.Number);
    }

    [Fact]
    public async Task ShouldReturnRequestedDentalOffices()
    {
        var d1 = _builder.WithName("cityOffice A").WithAddress("1;street;11111;city").WithDays(62).Build();
        var d2 = _builder.WithName("cityOffice B").WithAddress("3;street;11111;city").WithDays(62).Build();
        var d3 = _builder.WithName("Dental office A").WithAddress("4;street;11115;city2").WithDays(30).Build();
         
        _dbContext.DentalOffices.AddRange(d1, d2, d3);
        await _dbContext.SaveChangesAsync(_token);
        
        var offices = await _client.GetFromJsonAsync<List<DentalOfficeListDTO>>($"{_baseroute}?name=cityOffice&city=city&days=62",_token);

        Assert.Equal(2, offices.Count);
        Assert.True(offices.All(o=>o.Name.Contains("cityOffice")));
       
    }
    

 
}