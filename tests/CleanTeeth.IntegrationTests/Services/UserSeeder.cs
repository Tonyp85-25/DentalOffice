using Microsoft.AspNetCore.Identity;
using Security.Models;

namespace CleanTeeth.IntegrationTests.Services;

internal static class UserSeeder
{
    public static async Task SeedUsers(IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
        if (await userManager.FindByEmailAsync("admin@test.com") != null) return;
        var admin = new User
        {
            Email = "admin@test.com",
            UserName = "admin"
        };
        var password = "Secret123$";
        var result=await userManager.CreateAsync(admin, password);
        Assert.True(result.Succeeded);
        await userManager.AddToRoleAsync(admin, nameof(AppRoles.Admin));
        var employee = new User
        {
            Email = "employee@test.com",
            UserName = "employee"
        };
       result= await userManager.CreateAsync(employee, password);
       Assert.True(result.Succeeded);
        await userManager.AddToRoleAsync(employee, nameof(AppRoles.Employee));
    }
}