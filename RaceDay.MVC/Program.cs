// using Microsoft.EntityFrameworkCore; // Remove this line

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register HttpContextAccessor for layout session access
builder.Services.AddHttpContextAccessor();

// Register HttpClient to communicate with the Part 2 Web API (Updated to port 7107)
builder.Services.AddHttpClient("RaceDayAPI", client =>
{
    client.BaseAddress = new Uri("https://localhost:7107/");
});

// Add session support for storing authentication state and roles
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var builderApp = builder.Build();

// Configure the HTTP request pipeline.
if (!builderApp.Environment.IsDevelopment())
{
    builderApp.UseExceptionHandler("/Home/Error");
    builderApp.UseHsts();
}

// builderApp.UseHttpsRedirection();

// Serve CSS, JS, and image files from wwwroot
builderApp.UseStaticFiles();

builderApp.UseRouting();

// Enable session middleware before authorization
builderApp.UseSession();
builderApp.UseAuthorization();

builderApp.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

builderApp.Run();