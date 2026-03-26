using CleanTeeth.API.Middlewares;
using CleanTeeth.Application;
using CleanTeeth.Persistence;
using Security;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddProblemDetails();
builder.Services.AddControllers();

var connectionString = builder.Configuration.GetDBString();
builder.Services.AddPersistenceServices(connectionString);
builder.Services.AddApplicationServices();
builder.Services.AddSecurityServices(connectionString);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();


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

app.Run();

public partial class Program { }