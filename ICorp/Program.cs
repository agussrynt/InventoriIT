using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using InventoryIT.Data;
using InventoryIT.Areas.Master.Interface;
using InventoryIT.Areas.Master.Service;
using InventoryIT.Models;
using InventoryIT.Areas.Page.Interfaces;
using InventoryIT.Areas.Page.Services;
using InventoryIT.Middlewares;
using InventoryIT.Areas.Page.Models;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("PlanCorpDbContextConnection") ?? throw new InvalidOperationException("Connection string 'PlanCorpDbContextConnection' not found.");

builder.Services.AddDbContext<PlanCorpDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<PlanCorpDbContext>()
    .AddDefaultTokenProviders();
//.AddDefaultUI();

// Add collection services Master Data
builder.Services.AddScoped<IWorkService, WorkService>();
builder.Services.AddScoped<IProjectService, ProjectService>();

// Add collection Services Revenue
builder.Services.AddScoped<IRevenueService, RevenueService>();


builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IYearService, YearService>();
builder.Services.AddScoped<IFungsiService, FungsiService>();
builder.Services.AddScoped<IDokumenUPService, DokumenUPService>();
builder.Services.AddScoped<IAuditProcessService, AuditProcessService>();
builder.Services.AddScoped<IFollowUpService, FollowUpService>();
builder.Services.AddScoped<IMailService, MailService>();
builder.Services.AddScoped<IAuditExternalService, AuditExternalService>();
builder.Services.AddScoped<IAuditExternalAssigmentService, AuditExternalAssigmentService>();
builder.Services.AddScoped<IAuditExternalFolowUpService, AuditExternalFolowUpService>();
builder.Services.AddScoped<IAuditExternalMonitoring, AuditExternalMonitoring>();

builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IUserInRoleService, UserInRoleService>();

// Add services to the container.
builder.Services.AddApplicationInsightsTelemetry(builder.Configuration.GetSection("ApplicationInsights:InstrumentationKey")); // add App insight telematry
builder.Services.AddSingleton<ConnectionDB>();
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

//New add belum di tes
//builder.Services.AddTransient<IUserInRoleService, UserInRoleService>();

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(typeof(ExceptionHandlerAttribute));
}).AddRazorRuntimeCompilation();

builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings.
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 3;
    //options.Password.RequiredUniqueChars = 1;

    // Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(30);
    options.Lockout.MaxFailedAccessAttempts = 3;
    options.Lockout.AllowedForNewUsers = true;

    // User settings.
    options.User.AllowedUserNameCharacters =
    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = true;
});

// Add our Config object so it can be injected
builder.Services.Configure<Setting>(builder.Configuration.GetSection("Setting"));

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(480);

    options.LoginPath = "/account/login";
    //options.AccessDeniedPath = new PathString("/access-denied");
    options.SlidingExpiration = true;
});

builder.Services.AddHttpClient();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(480);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();
app.UseAuthenticationMiddlewareCustom();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "areaRoute",
    pattern: "{area:exists}/{controller}/{action}"
);

//app.MapRazorPages();
app.Run();