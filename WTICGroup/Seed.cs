using Microsoft.AspNetCore.Identity;
using WTICGroup.ViewModels;

namespace WTICGroup;

public class Seed 
{
     public static async Task IntializeRolesAsync(IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices
            .GetRequiredService<IServiceScopeFactory>()
            .CreateScope();

        var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger<Seed>();

        var roles = config.GetSection("Identity:IdentityServer:Roles").Get<List<string>>(); 

        foreach(var role in roles)
        {
            if(!await roleManager.RoleExistsAsync(role.ToLowerInvariant()))
            {
                var newRole = new IdentityRole() { Name = role.ToLowerInvariant() };
                var result = await roleManager.CreateAsync(newRole);

                if(!result.Succeeded)
                {
                    logger.LogWarning("Seed role {role} failed.", result.Errors);
                }
                else
                {
                    logger.LogInformation("Seed role {} succeeded.", role);
                }
            }
        }

        logger.LogInformation("Seeding roles is complete.");
    }

    public static async Task InitializeTestUsers(IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices
            .GetRequiredService<IServiceScopeFactory>()
            .CreateScope();

        var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger<Seed>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();

        var users = config.GetSection("Identity:IdentityServer:TestUsers").Get<List<SeedUser>>();

        foreach (var user in users)
        {
            var appUser = new IdentityUser
            {
                UserName = user.UserName
            };
            
            var result = await userManager.CreateAsync(appUser, user.Password);

            if (result.Succeeded)
            {
                logger.LogInformation("User {} is created", appUser.UserName);

                var roleResult = await userManager.AddToRolesAsync(appUser, user.Roles);

                if(roleResult.Succeeded)
                {
                    logger.LogInformation("User added to roles {}", string.Join(',', user.Roles ?? Array.Empty<string>()));
                }
                else
                {
                    logger.LogWarning("User could not be added to roles {}", string.Join(',', user.Roles ?? Array.Empty<string>()));
                }
            }
            else
            {
                logger.LogWarning("User {} could not be created", appUser.UserName);
            }
        }
    }
}
