using CleanTeeth.API.Middlewares;
using CleanTeeth.Application;
using CleanTeeth.Persistence;
using Security;
using Security.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddProblemDetails();
builder.Services.AddControllers();

var connectionString = builder.Configuration.GetAppDbString();
builder.Services.AddSecurityServices(connectionString);
builder.Services.AddPersistenceServices(connectionString);
builder.Services.AddApplicationServices();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

app.MapIdentityApi<User>();

app.UseCustomExceptionHandler();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

if (!app.Environment.IsEnvironment("testing"))
{
    using (var scope = app.Services.CreateScope())
    {
        await IdentitySeeder.SeedRolesAsync(scope.ServiceProvider);
    }
}


await app.RunAsync();

public partial class Program
{
    protected Program() { }
}
