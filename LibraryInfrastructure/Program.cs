using LibraryInfrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using LibraryDomain.Model;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Підключення SocialNetworkContext з використанням SQL Server
builder.Services.AddDbContext<SocialNetworkContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Налаштування Identity із підтримкою ролей
builder.Services
    .AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>() // Додаємо підтримку ролей
    .AddEntityFrameworkStores<SocialNetworkContext>();

builder.Services.AddRazorPages();

var app = builder.Build();

// Сідування базових даних: створення ролі "Admin" та адміністратора
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = services.GetRequiredService<UserManager<User>>();

        string roleName = "Admin";
        // Створення ролі Admin, якщо її немає
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }

        // Створення адміністратора з даними: admin@gmail.com / Abcd12345
        string adminEmail = "admin@gmail.com";
        string adminPassword = "Nmgnx7xsU4ic#c.";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser == null)
        {
            adminUser = new User { UserName = adminEmail, Email = adminEmail };
            var result = await userManager.CreateAsync(adminUser, adminPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, roleName);
            }
        }
    }
    catch (Exception ex)
    {
        // Тут можна залогувати помилку
        throw;
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/User/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();

// Якщо використовуєте статичні файли
app.MapStaticAssets();

// Маршрут за замовчуванням → Users/Index
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Users}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
