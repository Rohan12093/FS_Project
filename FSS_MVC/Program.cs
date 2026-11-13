using FSS_MVC.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();


builder.Services.AddHttpClient("FSS_API", client =>
{
    client.BaseAddress = new Uri("https://localhost:7218/");
});

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.Name = "UniversitySession";
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

//--Kailash 11/11/2026--//
using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider
        .GetRequiredService<UserManager<ApplicationUser>>();

    var users = new (string Email, string Role)[]
{
        ("admin@example.com","Admin"),
        ("patient@example.com","Patient"),
        ("doctor@example.com","Doctor")
    };

    foreach (var item in users)
    {
        var user = await userManager.FindByEmailAsync(item.Email);

        if (user == null)
        {
            user = new ApplicationUser
            {
                UserName = item.Email,
                Email = item.Email,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(
                user, "Password123!");

            if (!result.Succeeded)
            {
                throw new Exception(
                    string.Join(", ",
                        result.Errors.Select(e => e.Description)));
            }
        }

        if (!await userManager.IsInRoleAsync(user, item.Role))
        {
            var roleResult = await userManager.AddToRoleAsync(
                user, item.Role);

            if (!roleResult.Succeeded)
            {
                throw new Exception(
                    string.Join(", ",
                        roleResult.Errors.Select(e => e.Description)));
            }
        }
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseSession();
app.UseAuthentication(); 
app.UseAuthorization();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages()
   .WithStaticAssets();

app.Run();