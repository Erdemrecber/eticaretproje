using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using eticaret.Data;
using Microsoft.AspNetCore.Identity;
using eticaret.Services; // CartService'i dahil etmek i�in
using Microsoft.AspNetCore.Http; // StatusCodes kullan�m� i�in

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Default Identity ve rol y�netimi
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false; // E-posta onay� zorunlu olmas�n
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
})
    .AddRoles<IdentityRole>() // Rol y�netimi i�in
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews(); // MVC hizmetlerini ekle

// Add CartService to the service container
builder.Services.AddScoped<CartService>();

builder.Services.AddHttpContextAccessor(); // HttpContext'e eri�im sa�lamak i�in gerekli

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
    app.UseStatusCodePages(async context =>
    {
        var response = context.HttpContext.Response;
        if (response.StatusCode == StatusCodes.Status401Unauthorized)
        {
            response.Redirect("/Account/Login");
        }
    });
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers(); // MVC denetleyicilerini haritaland�r

// Rolleri ve admin kullan�c�s�n� olu�tur
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await SeedData.SeedRolesAndAdmin(services); // SeedData'y� burada �a��r�n
}

app.Run();

public static class SeedData
{
    public static async Task SeedRolesAndAdmin(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

        // Admin rol� varsa olu�turmay�n
        if (!await roleManager.RoleExistsAsync("Admin"))
        {
            await roleManager.CreateAsync(new IdentityRole("Admin"));
        }

        // admin ile oturum a�ma
        var adminEmail = "admin@example.com"; // email
        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser == null)
        {
            adminUser = new IdentityUser { UserName = adminEmail, Email = adminEmail };
            await userManager.CreateAsync(adminUser, "AdminPassword123!"); // �ifre
        }

        if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
        {
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }
}
