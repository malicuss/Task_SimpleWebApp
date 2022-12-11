using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using SimpleWebApp.Core.Helpers;
using SimpleWebApp.Core.Middleware;
using SimpleWebApp.Core.Models;
using SimpleWebApp.Core.Services;
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

builder.Host.UseSerilog();
builder.Services.AddDbContext<NorthwindContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
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

builder.Services.AddSwaggerGen(x =>
{
    x.SwaggerDoc("v1",new OpenApiInfo{ Title = "Simple Api", Version = "v1.2"});
});
builder.Services.AddSwaggerGenNewtonsoftSupport();
builder.Services.AddMvcCore().AddApiExplorer();

var app = builder.Build();

if (!app.Environment.IsDevelopment() ||
    !app.Configuration.GetValue<bool>("ExceptionHandlingDev"))
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
    app.UseStatusCodePages();
}

app.Logger.Log(LogLevel.Information,Environment.CurrentDirectory);


#if Apach || Nginx

#else
app.UseHttpsRedirection();

#endif

app.UseStaticFiles();
app.UseSerilogRequestLogging();
app.UseRouting();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.UseMiddleware<ImageCache>();

app.UseSwagger();
app.UseSwaggerUI(x =>
{
    x.SwaggerEndpoint("v1/swagger.json", "Simple Api v1");
});

app.Run();

public partial class Program { }