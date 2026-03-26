using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Security.Models;

namespace Security;

public static class RegisterSecurityServices
{
    public static void AddSecurityServices(this IServiceCollection services, string connectionString)
    {
        services.AddAuthentication(IdentityConstants.BearerScheme).AddBearerToken(IdentityConstants.BearerScheme);
        
        services.AddAuthorization();
        services.AddDbContext<SecurityDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddIdentityCore<User>()
            .AddEntityFrameworkStores<SecurityDbContext>()
            .AddApiEndpoints();
    }
}