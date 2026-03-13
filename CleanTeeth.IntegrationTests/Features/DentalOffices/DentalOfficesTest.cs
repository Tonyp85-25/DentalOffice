
using System.Net;
using CleanTeeth.API.DTO.DentalOffices;
using CleanTeeth.Application.Features.DentalOffices.Queries;
using CleanTeeth.Fixtures.Entities;
using CleanTeeth.IntegrationTests.Fixtures;
using Microsoft.AspNetCore.Mvc;

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
        var dentalOffice = new 
            { Name = "Test", Street = "street",Zipcode = "11111", City = "city", Number = "1",Days = new List<int>{2,4,8,16}};
        var content = JsonContent.Create(dentalOffice);
        var response = await _client.PostAsync($"{_baseroute}", content,_token );
        
        Assert.Equal(HttpStatusCode.Created,response.StatusCode);
    }
    
    [Fact]
    public async Task InvalidDataShouldNotCreateDentalOffice()
    {
        var dentalOffice = new 
            { Street = "street",Zipcode = "111", City = "city", Number = "1"};
        var content = JsonContent.Create(dentalOffice);
        var response = await _client.PostAsync($"{_baseroute}", content,_token );
        var result = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>(_token);
        
        Assert.Equal((int)HttpStatusCode.BadRequest, result.Status);
        Assert.Equal(3, result.Errors.Count);
        Assert.Contains("The Name field is required.", result.Errors["Name"]);
        Assert.Contains("Zipcode must be 5 digits", result.Errors["Zipcode"]);
        Assert.Contains("The Days field is required.", result.Errors["Days"]);
    }
    
    [Fact]
    public async Task ShouldReadDentalOffice()
    {
        Guid id =Guid.NewGuid();
        var dentalOffice = _builder.WithGuid(id).WithName("Office A").WithAddress("1;street;11111;city").WithDays(62).Build();
         _dbContext.DentalOffices.Add(dentalOffice);
        await _dbContext.SaveChangesAsync(_token);
        
        var office = await _client.GetFromJsonAsync<DentalOfficeDetailDTO>($"{_baseroute}/{id}",_token);
            
        Assert.Equal("Office A", office?.Name);
        Assert.Equal("street", office.Street);
        Assert.Equal("11111", office.Zipcode);
        Assert.Equal("city", office.City);
        Assert.Equal("1", office.Number);
    }
    
    [Fact]
    public async Task InvalidIdShouldReturnNotFound()
    {
        Guid id =Guid.Empty;
        var dentalOffice = _builder.WithGuid(Guid.NewGuid()).WithName("Office A").WithAddress("1;street;11111;city").WithDays(62).Build();
        _dbContext.DentalOffices.Add(dentalOffice);
        await _dbContext.SaveChangesAsync(_token);
        
        var response = await _client.GetAsync($"{_baseroute}/{id}",_token);
        
        Assert.Equal(HttpStatusCode.NotFound,response.StatusCode);
    }

    [Fact]
    public async Task ShouldReturnRequestedDentalOffices()
    {
        var d1 = _builder.WithName("cityOffice A").WithAddress("1;street;11111;city").WithDays(62).Build();
        var d2 = _builder.WithName("cityOffice B").WithAddress("3;street;11111;city").WithDays(62).Build();
        var d3 = _builder.WithName("Dental office A").WithAddress("4;street;11115;city2").WithDays(30).Build();
         
        _dbContext.DentalOffices.AddRange(d1, d2, d3);
        await _dbContext.SaveChangesAsync(_token);
        
        var offices = await _client.GetFromJsonAsync<List<DentalOfficeListDTO>>($"{_baseroute}/search?name=cityOffice&city=city&days=2&days=4&days=8",_token);

        Assert.Equal(2, offices.Count);
        Assert.True(offices.All(o=>o.Name.Contains("cityOffice")));
       
    }
    

 
}