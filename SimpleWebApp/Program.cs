using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Serilog;
using SimpleWebApp.Helpers;
using SimpleWebApp.Middleware;
using SimpleWebApp.Models;
using SimpleWebApp.Services;
using SmartBreadcrumbs.Extensions;

//configuring Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs//Log.txt",
        rollingInterval: RollingInterval.Day,
        fileSizeLimitBytes: 1048576)
    .CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);

Log.Information(ConfigLoggingHelper.GetConfigString(builder.Configuration));

builder.Services.AddDbContext<NorthwindContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
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
var app = builder.Build();

if (!app.Environment.IsDevelopment() ||
    !app.Configuration.GetValue<bool>("ExceptionHandlingDev"))
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
    app.UseStatusCodePages();
}

app.Logger.Log(LogLevel.Information,Environment.CurrentDirectory);

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSerilogRequestLogging();

app.UseRouting();

app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.UseMiddleware<ImageCache>();
app.Run();

public partial class Program { }