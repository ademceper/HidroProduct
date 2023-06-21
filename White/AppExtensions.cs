using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.RegularExpressions;
using White.Data;

namespace White;

public static class AppExtensions
{
    public static async Task<IApplicationBuilder> UseWhite(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        using var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        using var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();
        using var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
        var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
        context.Database.Migrate();

        await roleManager.CreateAsync(new AppRole { Name = "Administrators" });
        await roleManager.CreateAsync(new AppRole { Name = "Görüntüleyiciler" });
        await roleManager.CreateAsync(new AppRole { Name = "Kaydediciler" });
        await roleManager.CreateAsync(new AppRole { Name = "Siliciler" });
        await roleManager.CreateAsync(new AppRole { Name = "Güncelleyiciler" });




        var user = new AppUser 
        {
           
            Name = configuration.GetValue<string>("DefaultUser:Name"),
            UserName = configuration.GetValue<string>("DefaultUser:Email"),
            Email = configuration.GetValue<string>("DefaultUser:Email"),
           

            EmailConfirmed = true
        };

        userManager.CreateAsync(user, configuration.GetValue<string>("DefaultUser:Password")).Wait();
        userManager.AddToRoleAsync(user, "Administrators").Wait();
        userManager.AddClaimAsync(user, new Claim("Name", user.Name)).Wait();

        return app;
    }
    public static string ToSafeUrlString(this string text) => Regex.Replace(string.Concat(text.Where(p => char.IsLetterOrDigit(p) || char.IsWhiteSpace(p))), @"\s+", "-");
}
