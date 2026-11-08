using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data;
using RaceDay.API.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<RaceDayDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Automatically wipe orphaned metadata, apply migrations, and seed default roles on startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<RaceDayDbContext>();

    // Wipes lingering LocalDB metadata to prevent Error 1801 and file locks
    dbContext.Database.EnsureDeleted();
    dbContext.Database.Migrate();

    // Automatically seed default roles with explicit IDs (1 and 2)
    if (!dbContext.Roles.Any())
    {
        dbContext.Roles.AddRange(
            new Role { RoleId = 1, RoleName = "Organiser" },
            new Role { RoleId = 2, RoleName = "Participant" }
        );
        dbContext.SaveChanges();
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();