using System.Net;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using SimpleWebApp.Core.Helpers;
using SimpleWebApp.Core.Middleware;
using SimpleWebApp.Core.Models;
using SimpleWebApp.Core.Services;
using SmartBreadcrumbs.Extensions;
using Microsoft.AspNetCore.Identity;
using SimpleWebApp;

//configuring Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs//Log.txt",
        rollingInterval: RollingInterval.Day,
        fileSizeLimitBytes: 1048576)
    .CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);

Log.Information(ConfigLoggingHelper.GetConfigString(builder.Configuration));

builder.Host.UseSerilog();
builder.Services.AddDbContext<NorthwindContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDbContext<SecurityContext>(opt=>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<SecurityContext>().AddDefaultUI().AddDefaultTokenProviders();
builder.Host.UseSerilog();
builder.Services.AddScoped<IDbContextWrapper, DbContextWrapper>();
builder.Services.AddSingleton<ICacher, ImageCacher>();
builder.Services.Configure<AppOptions>(builder.Configuration.GetSection(AppOptions.Options));
builder.Services.AddControllersWithViews(opt =>
{
    opt.Filters.Add<ActionLoggingFilter>();
});
builder.Services.AddBreadcrumbs(Assembly.GetExecutingAssembly(), opt =>
{
    opt.TagName = "nav";
    opt.TagClasses = "";
    opt.OlClasses = "breadcrumb";
    opt.LiClasses = "breadcrumb-item";
    opt.ActiveLiClasses = "breadcrumb-item active";
});

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    options.User.AllowedUserNameCharacters =
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = false;
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.SlidingExpiration = true;
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdministratorRole",
        policy => policy.RequireRole("Administrator"));
});

builder.Services.AddSwaggerGen(x =>
{
    x.SwaggerDoc("v1",new OpenApiInfo{ Title = "Simple Api", Version = "v1.2"});
});
builder.Services.AddSwaggerGenNewtonsoftSupport();
builder.Services.AddMvcCore().AddApiExplorer();

#if Apach || Nginx
builder.WebHost.ConfigureKestrel((c, so) =>
{
    serverOptions.Listen(IPAddress.Any, 8000, listenOptions =>
    {
        listenOptions.UseConnectionLogging();
    });
    so.Listen(IPAddress.Loopback, 5000);
    so.Listen(IPAddress.Loopback, 5001, lo =>
    {
        //lo.UseHttps("MySertificate.pfx", "MySertificatePassword");
    });
});
#endif
if (builder.Environment.IsDevelopment())
    builder.Services.AddWebOptimizer(minifyJavaScript:false,minifyCss:false);
else
    builder.Services.AddWebOptimizer(pipe =>
    {
        // minification
        pipe.MinifyJsFiles("js/site.js");
        pipe.MinifyCssFiles("css/**/*.css");
        
        // bundling

        pipe.AddCssBundle("/css/bundle.css", "/css/**/*.css");
    });

builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

var app = builder.Build();

using (var scoped = app.Services.CreateScope())
{
    var serviceProvider = scoped.ServiceProvider;
    var context = serviceProvider.GetRequiredService<SecurityContext>();
    var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var adminRole = new IdentityRole { Name = "Administrator", NormalizedName = "ADMINISTRATOR" };

    if (!await roleManager.RoleExistsAsync(adminRole.Name))
    {
        await context.Roles.AddAsync(adminRole);
        await roleManager.CreateAsync(adminRole);
        // creating default admin user;
        var adminUser = new IdentityUser { UserName = "sa@domain.com", Email = "sa@domain.com", NormalizedUserName = "sa@domain.com".ToUpper(),EmailConfirmed = true};
        await userManager.CreateAsync(adminUser, "123Qwert!");
        await userManager.AddToRoleAsync(adminUser, adminRole.Name);
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment() ||
    !app.Configuration.GetValue<bool>("ExceptionHandlingDev"))
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
    app.UseStatusCodePages();
}

app.Logger.Log(LogLevel.Information,Environment.CurrentDirectory);

app.UseWebOptimizer();

#if Apach || Nginx

#else
app.UseHttpsRedirection();

#endif

app.UseStaticFiles();
app.UseSerilogRequestLogging();
app.UseRouting();
app.UseAuthentication();;
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.UseMiddleware<ImageCache>();

app.UseSwagger();
app.UseSwaggerUI(x =>
{
    x.SwaggerEndpoint("v1/swagger.json", "Simple Api v1");
});

app.Run();

public partial class Program { }