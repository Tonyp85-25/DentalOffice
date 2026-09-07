using Projects;

var builder = DistributedApplication.CreateBuilder(args);

#pragma warning disable S6966

var db = builder.AddSqlServer("db")
        .WithLifetime(ContainerLifetime.Persistent)
        .AddDatabase("CleanTeeth")
    ;
var migrations = builder.AddProject<CleanTeeth_MigrationService>("migrations")
    .WithReference(db)
    .WaitFor(db)
    ;

builder.AddProject<CleanTeeth_API>("webapi")
    .WithReference(db)
    .WithReference(migrations)
    .WaitForCompletion(migrations);

builder.Build().Run();
#pragma warning restore S6966
