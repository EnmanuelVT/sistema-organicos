using Microsoft.AspNetCore.Identity;

namespace API.Seed;

public static class RoleSeeder 
{
    public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
    {
        string[] roleNames = { "Solicitante", "Analista", "Evaluador", "Admin" };
        foreach (var roleName in roleNames)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }
    }
}