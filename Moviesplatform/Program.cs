using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moviesplatform.Areas.Identity.Data;
using Moviesplatform.Data;
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("MoviesplatformDBContextConnection") ?? throw new InvalidOperationException("Connection string 'MoviesplatformDBContextConnection' not found.");

builder.Services.AddDbContext<MoviesplatformDBContext>(options => options.UseSqlServer(connectionString));

//builder.Services.AddDefaultIdentity<MoviesplatformUser>(options => options.SignIn.RequireConfirmedAccount = false).AddEntityFrameworkStores<MoviesplatformDBContext>();

builder.Services.AddIdentity<MoviesplatformUser, IdentityRole>(options =>
    options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<MoviesplatformDBContext>()
    .AddDefaultTokenProviders();
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
