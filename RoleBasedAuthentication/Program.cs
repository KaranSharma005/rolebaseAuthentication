using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using RoleBasedAuthentication.Data;
using RoleBasedAuthentication.Interfaces;
using RoleBasedAuthentication.Models;
using RoleBasedAuthentication.RepoHelpers;
using RoleBasedAuthentication.Services;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => {
    options.SignIn.RequireConfirmedAccount = true;
    })
    .AddRoles<IdentityRole>()           //for adding role useful for securing route from being accessses by another user
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddNotyf(config => {
    config.DurationInSeconds = 2;
    config.IsDismissable = true;
    config.Position = NotyfPosition.TopRight;
});
builder.Services.Configure<EmailSettingsModal>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<UsersHelper>();
builder.Services.AddTransient<ClassHelper>();
builder.Services.AddTransient<SpaHelper>();
builder.Services.AddTransient<IEmailWithAttachment, EmailSender>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseNotyf();
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
