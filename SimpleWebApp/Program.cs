using Microsoft.EntityFrameworkCore;
using Serilog;
using SimpleWebApp.Helpers;
using SimpleWebApp.Middleware;
using SimpleWebApp.Models;
using SimpleWebApp.Services;

//configuring Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs//Log.txt",
        rollingInterval: RollingInterval.Day,
        fileSizeLimitBytes: 1048576)
    .CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);

Log.Information(ConfigLoggingHelper.GetConfigString(builder.Configuration));

// Add services to the container.

builder.Services.AddTransient<IDbContextWrapper, DbContextWrapper>();
builder.Services.AddDbContext<NorthwindContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Host.UseSerilog();  //inject Serilog
builder.Services.AddTransient<IDbContextWrapper, DbContextWrapper>();
builder.Services.AddTransient<ICacher, ImageCacher>();
builder.Services.Configure<AppOptions>(builder.Configuration.GetSection(AppOptions.Options));
builder.Services.AddControllersWithViews(opt =>
{
    opt.Filters.Add<ActionLoggingFilter>();
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment() ||
    !app.Configuration.GetValue<bool>("ExceptionHandlingDev"))
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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