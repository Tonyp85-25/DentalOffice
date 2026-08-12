
using System.Net;
using CleanTeeth.Application.Features.DentalOffices.Queries;
using CleanTeeth.Fixtures.Entities;
using CleanTeeth.IntegrationTests.Fixtures;
using Microsoft.AspNetCore.Mvc;
using Security.Models;

namespace CleanTeeth.IntegrationTests.Features.DentalOffices;


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
    public async Task ShouldCreateDentalOffice()
    {
        var  dentalOffice = new
            { Name = "Test", Street = "street",
                Zipcode = "11111", City = "city",
                Number = "1",
                Days = new List<int>{2,4,8,16}

            };


        var content = JsonContent.Create(dentalOffice);
        var admin =await UserManager.FindByEmailAsync("admin@test.com");
        Login(admin!.Id,AppRoles.Admin);
        var response = await Client.PostAsync(_baseUri, content,Token );

        Assert.Equal(HttpStatusCode.Created,response.StatusCode);
    }

    [Fact]
    public async Task OnlyAdminShouldCreateDentalOffice()
    {
        var dentalOffice = new
            { Name = "Test", Street = "street",Zipcode = "11111", City = "city", Number = "1",Days = new List<int>{2,4,8,16}};
        var content = JsonContent.Create(dentalOffice);
        var employee =await UserManager.FindByEmailAsync("employee@test.com");
     Assert.NotNull(employee);
        Login(employee.Id,AppRoles.Employee);
        var response = await Client.PostAsync(_baseUri, content,Token );

        Assert.Equal(HttpStatusCode.Forbidden,response.StatusCode);
    }

    [Fact]
    public async Task InvalidDataShouldNotCreateDentalOffice()
    {
        var dentalOffice = new
            { Street = "street",Zipcode = "111", City = "city", Number = "1"};
        var content = JsonContent.Create(dentalOffice);
        var admin =await UserManager.FindByEmailAsync("admin@test.com");
        Assert.NotNull(admin);
        Login(admin.Id,AppRoles.Admin);
        var response = await Client.PostAsync(_baseUri, content,Token );
        var result = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>(Token);
        Assert.NotNull(result);

        Assert.Equal((int)HttpStatusCode.BadRequest, result.Status);
        Assert.Equal(3, result.Errors.Count);
        Assert.Contains("The Name field is required.", result.Errors["Name"], StringComparer.Ordinal);
        Assert.Contains("Zipcode must be 5 digits", result.Errors["Zipcode"], StringComparer.Ordinal);
        Assert.Contains("The Days field is required.", result.Errors["Days"], StringComparer.Ordinal);
    }

    [Fact]
    public async Task ShouldReadDentalOffice()
    {
        Guid id =Guid.NewGuid();
        var dentalOffice = _builder.WithGuid(id).WithName("Office A").WithAddress("1;street;11111;city").WithDays(62).Build();
         DbContext.DentalOffices.Add(dentalOffice);
        await DbContext.SaveChangesAsync(Token);
        var employee =await UserManager.FindByEmailAsync("employee@test.com");
        Assert.NotNull(employee);

        Login(employee.Id,AppRoles.Employee);
        var office = await Client.GetFromJsonAsync<DentalOfficeDetailDto>($"{_baseroute}/{id}",Token);

        Assert.NotNull(office);
        Assert.Equal("Office A", office.Name);
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
        DbContext.DentalOffices.Add(dentalOffice);
        await DbContext.SaveChangesAsync(Token);
        var employee =await UserManager.FindByEmailAsync("employee@test.com");
        Assert.NotNull(employee);
        Login(employee.Id,AppRoles.Employee);
        var response = await Client.GetAsync(new Uri($"{_baseUri.AbsoluteUri}/{id}"),Token);

        Assert.Equal(HttpStatusCode.NotFound,response.StatusCode);
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
        var response = await Client.GetAsync(new Uri($"{_baseUri.AbsoluteUri}/search?name=cityOffice&city=city&days=2&days=4&days=8"),Token);

        Assert.Equal(HttpStatusCode.OK,response.StatusCode);
        var offices = await response.Content.ReadFromJsonAsync<List<DentalOfficeListDto>>(Token);
        Assert.NotNull(offices);
        Assert.Equal(2, offices.Count);
        Assert.True(offices.All(o=>o.Name.Contains("cityOffice", StringComparison.Ordinal)));

    }



}
