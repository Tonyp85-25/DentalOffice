using CleanTeeth.MigrationService;
using CleanTeeth.Persistence;
using Security;
using ServiceDefaults;

var builder = Host.CreateApplicationBuilder(args);
builder.AddServiceDefaults();
builder.Services.AddHostedService<Worker>();

builder.Services.AddOpenTelemetry()
    .WithTracing(tracing => tracing.AddSource(Worker.ActivitySourceName));

builder.AddSqlServerDbContext<CleanTeethDbContext>("CleanTeeth");
builder.AddSqlServerDbContext<SecurityDbContext>("CleanTeeth");

var host = builder.Build();
host.Run();
