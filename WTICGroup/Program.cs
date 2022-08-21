using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WTICGroup;
using WTICGroup.Data;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<WTICGroupContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("WTICGroupContext") ?? 
                        throw new InvalidOperationException("Connection string 'WTICGroupContext' not found.")));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
    {
        options.Password.RequiredLength = 6;
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = false;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<WTICGroupContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


await Seed.IntializeRolesAsync(app);
await Seed.InitializeTestUsers(app);

app.Run();
