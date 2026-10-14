using System.Net;
using CleanTeeth.Application.Features;
using CleanTeeth.Fixtures.Entities;
using CleanTeeth.IntegrationTests.Fixtures;
using Security.Models;

namespace CleanTeeth.IntegrationTests.Features.PatientApp.DentalOffices;


public sealed class DentalOfficesTest:  BaseIntegrationTest,IClassFixture<TestContainersFixture>
{

    private readonly string _baseroute = "api/dentaloffices";

    private readonly DentalOfficeBuilder _builder = new();
    private  Uri _baseUri;

    public DentalOfficesTest(TestContainersFixture fixture) : base(fixture)
    {


    }

    public override async ValueTask InitializeAsync()
    {
        await base.InitializeAsync();
        _baseUri = new Uri(Client.BaseAddress!, _baseroute);
    }





    [Fact]
    public async Task ShouldReturnRequestedDentalOffices()
    {
        var d1 = _builder.WithName("cityOffice A").WithAddress("1;street;11111;city").WithDays(62).Build();
        var d2 = _builder.WithName("cityOffice B").WithAddress("3;street;11111;city").WithDays(62).Build();
        var d3 = _builder.WithName("Dental office A").WithAddress("4;street;11115;city2").WithDays(30).Build();

        DbContext.DentalOffices.AddRange(d1, d2, d3);
        await DbContext.SaveChangesAsync(Token);

        var employee =await UserManager.FindByEmailAsync("employee@test.com");
        Assert.NotNull(employee);

        Login(employee.Id,AppRoles.Employee);
        var response = await Client.GetAsync(new Uri($"{_baseUri.AbsoluteUri}?name=cityOffice&city=city&days=2&days=4&days=8"),Token);

        Assert.Equal(HttpStatusCode.OK,response.StatusCode);
        var offices = await response.Content.ReadFromJsonAsync<List<DentalOfficeListDto>>(Token);
        Assert.NotNull(offices);
        Assert.Equal(2, offices.Count);
        Assert.True(offices.All(o=>o.Name.Contains("cityOffice", StringComparison.Ordinal)));

    }



}
