using Projects;

var builder = DistributedApplication.CreateBuilder(args);

#pragma warning disable S6966

var db = builder.AddSqlServer("db")
        .WithLifetime(ContainerLifetime.Persistent)



    ;


builder.AddProject<CleanTeeth_API>("webapi").WithReference(db.AddDatabase("CleanTeeth")).WaitFor(db);
builder.Build().Run();
#pragma warning restore S6966
