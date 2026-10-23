using System.Net;
using CleanTeeth.Domain.Entities;
using CleanTeeth.Domain.ValueObjects;
using CleanTeeth.Fixtures.Entities;
using CleanTeeth.IntegrationTests.Fixtures;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Security.Models;

namespace CleanTeeth.IntegrationTests.Features.BackOffice.Dentists;

public sealed class DentistsTest : BaseIntegrationTest, IClassFixture<TestContainersFixture>
{
    public DentistsTest(TestContainersFixture fixture) : base(fixture)
    {
    }

    private Uri _baseUri;

    private readonly string _baseroute = "api/admin/dentists";
    private readonly DentalOfficeBuilder _builder = new();


    public override async ValueTask InitializeAsync()
    {
        await base.InitializeAsync();
        _baseUri = new Uri(Client.BaseAddress!, _baseroute);
    }

    [Fact]
    public async Task ShouldCreateDentist()
    {
        var id1 = Guid.NewGuid();
        var id2 = Guid.NewGuid();
        var office1 = _builder.WithGuid(id1).WithName("Office A").WithAddress("1;street;11111;city").WithDays(62)
            .Build();
        var office2 = _builder.WithGuid(id2).WithName("Office B").WithAddress("2;street;22222;city").WithDays(62)
            .Build();

        office1.Publish();
        office2.Publish();
        DbContext.DentalOffices.Add(office1);
        DbContext.DentalOffices.Add(office2);
        await DbContext.SaveChangesAsync(Token);

        var dentist = new
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            Offices = new List<string> { id1.ToString(), id2.ToString() },
        };
        var content = JsonContent.Create(dentist);
        var admin = await UserManager.FindByEmailAsync("admin@test.com");
        Assert.NotNull(admin);
        Login(admin.Id, AppRoles.Admin);
        var response = await Client.PostAsync(_baseUri, content, Token);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var created = await DbContext.Dentists.Include(d=>d.DentalOffices).FirstOrDefaultAsync(d => d.Email == new Email(dentist.Email), Token);
        Assert.NotNull(created);
        Assert.Equal(dentist.FirstName, created.FirstName);
        Assert.Equal(dentist.LastName, created.LastName);
        Assert.Equal(dentist.Email, created.Email.Value);
        Assert.Equal(dentist.Offices, created.DentalOffices.Select(d => d.Id.ToString()).ToList());
    }

    [Fact]
    public async Task ShouldNotCreateDentistIfNotAdmin()
    {
        var id1 = Guid.Empty;
        var dentist = new
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "test@example.com",
            Offices = new List<string> { id1.ToString() },
        };
        var content = JsonContent.Create(dentist);
        var employee = await UserManager.FindByEmailAsync("employee@test.com");
        Assert.NotNull(employee);
        Login(employee.Id, AppRoles.Employee);
        var response = await Client.PostAsync(_baseUri, content, Token);
        Assert.Equal(HttpStatusCode.Forbidden,response.StatusCode);
    }

    [Fact]
    public async Task ShouldNotCreateDentistWithUnpublishedOffices()
    {
        var id1 = Guid.NewGuid();
        var id2 = Guid.NewGuid();
        var office1 = _builder.WithGuid(id1).WithName("Office A").WithAddress("1;street;11111;city").WithDays(62)
            .Build();
        var office2 = _builder.WithGuid(id2).WithName("Office B").WithAddress("2;street;22222;city").WithDays(62)
            .Build();

        office1.Unpublish();
        office2.Unpublish();
        DbContext.DentalOffices.Add(office1);
        DbContext.DentalOffices.Add(office2);
        await DbContext.SaveChangesAsync(Token);

        var dentist = new
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "test2@example.com",
            Offices = new List<string> { id1.ToString(), id2.ToString() },
        };
        var content = JsonContent.Create(dentist);
        var admin = await UserManager.FindByEmailAsync("admin@test.com");
        Assert.NotNull(admin);
        Login(admin.Id, AppRoles.Admin);
        var response = await Client.PostAsync(_baseUri, content, Token);
        var result = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>(Token);
        Assert.NotNull(result);

        Assert.Equal((int)HttpStatusCode.NotFound, result.Status);
    }
}
