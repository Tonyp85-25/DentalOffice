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
        
        services.AddDbContext<SecurityDbContext>(options =>
            options.UseSqlServer(connectionString));

     
        services.AddAuthorization(
            options=>
            {
                options.AddPolicy("RequiresAdmin", policy =>
                {
                    policy.RequireRole(nameof(AppRoles.Admin));
                });
                options.AddPolicy("RequiresEmployee", policy =>
                {
                    policy.RequireRole(nameof(AppRoles.Employee));
                });
            });
        
        services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<SecurityDbContext>()
            .AddApiEndpoints();
        
        
        
    }
    
}