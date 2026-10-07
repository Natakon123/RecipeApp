
using Microsoft.EntityFrameworkCore;
using RecipeApp;
using RecipeApp.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// เปลี่ยนมาใช้ SQLite เพื่อให้เซิร์ฟเวอร์ Render สร้างไฟล์ฐานข้อมูลได้ทันทีโดยไม่ต้องต่อ SQL Server ภายนอก
var connString = builder.Configuration.GetConnectionString("DefaultConnection") 
                 ?? "Data Source=recipeapp.db";

builder.Services.AddDbContext<AppDbContext>(options => 
    options.UseSqlite(connString));

builder.Services.AddRazorPages();

builder.Services.AddScoped<PhotoService>();
builder.Services.AddScoped<RecipeService>();

var app = builder.Build();

// Auto Migration: สั่งสร้าง Database และ Tables อัตโนมัติทันทีที่เซิร์ฟเวอร์เริ่มทำงาน
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        context.Database.EnsureCreated(); // สร้างฐานข้อมูลและโครงสร้างตารางให้อัตโนมัติ
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred creating the DB.");
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
