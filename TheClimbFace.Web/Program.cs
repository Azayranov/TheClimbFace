using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TheClimbFace.Data;
using TheClimbFace.Data.Models;
using TheClimbFace.Data.Repository.Interfaces;
using TheClimbFace.Services.Data;
using TheClimbFace.Services.Data.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Configure the database connection
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString, a => a.MigrationsAssembly("TheClimbFace.Data")));

// Enable detailed exception pages for database-related errors in development
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
    {
        // Sign-in options
        options.SignIn.RequireConfirmedAccount = false; // Disable email confirmation for login

        // Password options
        options.Password.RequireUppercase = false; // Allow passwords without uppercase letters
        options.Password.RequireDigit = false; // Allow passwords without digits
        options.Password.RequireLowercase = false; // Allow passwords without lowercase letters
        options.Password.RequireNonAlphanumeric = false; // Allow passwords without special characters
    })
    .AddRoles<IdentityRole<Guid>>() // Add support for roles
    .AddSignInManager<SignInManager<ApplicationUser>>() // Add SignInManager for login/logout
    .AddUserManager<UserManager<ApplicationUser>>() // Add UserManager for user management
    .AddEntityFrameworkStores<ApplicationDbContext>(); // Use CinemaDbContext for Identity

// Add support for MVC controllers and Razor Pages
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Register repositories for dependency injection
builder.Services.AddScoped<IRepository<ClimbingCompetition>, BaseRepository<ClimbingCompetition>>();
builder.Services.AddScoped<IRepository<Boulder>, BaseRepository<Boulder>>();
builder.Services.AddScoped<IRepository<Club>, BaseRepository<Club>>();
builder.Services.AddScoped<IRepository<Climber>, BaseRepository<Climber>>();
builder.Services.AddScoped<IRepository<ClimberBoulderQualification>, BaseRepository<ClimberBoulderQualification>>();
builder.Services.AddScoped<IRepository<ApplicationUser>, BaseRepository<ApplicationUser>>();




// Register application services
builder.Services.AddScoped<IHomeService, HomeService>();
builder.Services.AddScoped<ICompetitionService, CompetitionService>();
builder.Services.AddScoped<IClimberService, ClimberService>();
builder.Services.AddScoped<IClubService, ClubService>();
builder.Services.AddScoped<IArbitratorService, ArbitratorService>();
builder.Services.AddScoped<IBoulderScoringService, BoulderScoringService>();


// Configure cookie authentication settings
/*
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login"; // Path to the login page
    options.LogoutPath = "/Identity/Account/Logout"; // Path to the logout page
    options.AccessDeniedPath = "/Identity/Account/AccessDenied"; // Path to the access denied page
    options.Cookie.SameSite = SameSiteMode.None; // Allow cross-site cookies (adjust if needed)
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Enforce HTTPS for cookies
    options.Cookie.HttpOnly = true; // Prevent client-side access to cookies
});

*/


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
    app.UseExceptionHandler("/Error/500");
    app.UseStatusCodePagesWithReExecute("/Error/{0}");
}
else
{
    app.UseExceptionHandler("Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Enable authentication middleware
app.UseAuthorization(); // Enable authorization middleware

// Configure default route for controllers
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Map Razor Pages
app.MapRazorPages();

app.Run();
