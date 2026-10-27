using GroupHomework66.Models;
using Microsoft.AspNetCore.Identity;

namespace groupwork71.BLL.Services;

public class AdminInitializer
{
    public static async Task SeedAdminAndRoleData(RoleManager<IdentityRole<int>> _roleManager, UserManager<User> _userManager)
    {
        string AdminEmail = "admin@admin.admin";
        string AdminPassword = "Admin123_";
        var roles = new []{"Admin", "User", "Manager"};
        foreach (var role in roles)
        {
            if(await _roleManager.FindByNameAsync(role) is null)
                await _roleManager.CreateAsync(new IdentityRole<int>(role));
        }

        if (await _userManager.FindByEmailAsync(AdminEmail) is null)
        {
            User admin = new User()
            {
                Email = AdminEmail,
                UserName = "Admin",
                EmailConfirmed = true,
                ProfilePictureUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcT7usbKcdZpIXUfeOoYoXnIoK7Ar6OwJfeuxLT2gQLT0AMjzV5D2_URKJc&s=10",
                DateOfBirth = DateTime.UtcNow.AddYears(-30),
            };
            IdentityResult result = await _userManager.CreateAsync(admin, AdminPassword);
            if (result.Succeeded)
                await _userManager.AddToRoleAsync(admin, "Admin");
        }
    }
}