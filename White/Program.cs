using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NETCore.MailKit.Extensions;
using NETCore.MailKit.Infrastructure.Internal;
using White;
using White.Data;
using static Org.BouncyCastle.Math.EC.ECCurve;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(config =>
{
    config.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSql"));

    config.UseLazyLoadingProxies();

});
builder.Services.AddIdentity<AppUser, AppRole>(config =>
{
    config.Password.RequiredLength = builder.Configuration.GetValue<int>("Password:RequiredLength");
    config.Password.RequireLowercase = builder.Configuration.GetValue<bool>("Password:RequireLowercase");
    config.Password.RequireUppercase = builder.Configuration.GetValue<bool>("Password:RequireUppercase");
    config.Password.RequireNonAlphanumeric = builder.Configuration.GetValue<bool>("Password:RequireNonAlphanumeric");
    config.Password.RequireDigit = builder.Configuration.GetValue<bool>("Password:RequireDigit");
    config.Password.RequiredUniqueChars = builder.Configuration.GetValue<int>("Password:RequiredUniqueChars");

    config.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    config.Lockout.MaxFailedAccessAttempts = 5;

    config.SignIn.RequireConfirmedEmail = true;

    config.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultEmailProvider;

})

    .AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddMailKit(optionBuilder =>
{
    optionBuilder.UseMailKit(new MailKitOptions()
    {
        //get options from sercets.json
        Server = builder.Configuration.GetValue<string>("EMail:Server"),
        Port = builder.Configuration.GetValue<int>("EMail:Port"),
        SenderName = builder.Configuration.GetValue<string>("EMail:SenderName"),
        SenderEmail = builder.Configuration.GetValue<string>("EMail:SenderEmail"),

        // can be optional with no authentication 
        Account = builder.Configuration.GetValue<string>("EMail:SenderEmail"),
        Password = builder.Configuration.GetValue<string>("EMail:Password"),
        // enable ssl or tls
        Security = builder.Configuration.GetValue<bool>("EMail:SslEnable")
    });
});


builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseWhite();

app.MapControllerRoute(
     name: "areas",
     pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}" );

app.MapControllerRoute(
    name: "Category",
    pattern: "{name}-c-{id}",
    defaults: new { controller = "Home", action = "Category" }
    );

app.MapControllerRoute(
    name: "Product",
    pattern: "{name}-p-{id}",
    defaults: new { controller = "Home", action = "Product" }
    );


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
