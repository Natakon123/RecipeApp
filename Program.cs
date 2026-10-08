
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RecipeApp;
using RecipeApp.Data;

var builder = WebApplication.CreateBuilder(args);

// 1. ตั้งค่า DbContext สำหรับ SQLite
builder.Services.AddDbContext<AppDbContext>(options => 
    options.UseSqlite("Data Source=recipeapp.db"));

// 2. เพิ่มบริการ ASP.NET Core Identity สำหรับระบบ Login / Register
builder.Services.AddDefaultIdentity<IdentityUser>(options => {
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
})
.AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddRazorPages();

builder.Services.AddScoped<PhotoService>();
builder.Services.AddScoped<RecipeService>();

var app = builder.Build();

// Auto Database Creation (สร้างตารางทั้ง Recipes และ Identity Tables อัตโนมัติ)
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

// 3. เพิ่ม Authentication ก่อน Authorization (จำเป็นมากสำหรับระบบ Login)
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
