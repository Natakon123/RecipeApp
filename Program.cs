// Natakon Wongnikom 671410014
using Microsoft.EntityFrameworkCore;
using RecipeApp;
using RecipeApp.Data;

var builder = WebApplication.CreateBuilder(args);

// กำหนด Connection String สำหรับ SQLite โดยตรง (ไม่พึ่ง appsettings.json)
builder.Services.AddDbContext<AppDbContext>(options => 
    options.UseSqlite("Data Source=recipeapp.db"));

builder.Services.AddRazorPages();

builder.Services.AddScoped<PhotoService>();
builder.Services.AddScoped<RecipeService>();

var app = builder.Build();

// Auto Database Creation
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        context.Database.EnsureCreated();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred creating the DB.");
    }
}


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
