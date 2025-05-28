using DotNet.DataAccess.Data;
using DotNet.DataAccess.Repository;
using DotNet.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

//ASP.NET Core automatically registers ApplicationDbContext as Scoped behind the scenes. 
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddSingleton<IWebHostEnvironment>(builder.Environment);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

app.Run();




// Entry point of the application - Program.cs in minimal hosting model (introduced in .NET 6+)
// This file configures the application's services, middleware pipeline, and starts the web server

// Creates a new WebApplicationBuilder instance which provides the foundation for configuring the application
// It loads configuration from appsettings.json, environment variables, command line arguments, etc.

// Registers MVC services in the dependency injection container
// This enables controllers, views, model binding, validation, and other MVC features

// Builds the WebApplication instance with all registered services
// This finalizes the service configuration and prepares the application for middleware registration

// Configure the HTTP request pipeline (middleware chain)
// Middleware are executed in the order they are added to the pipeline

// Different middleware configuration based on environment
// Checks environmentVariables inside Properties->launchSettings.json (by default, under https profile)
// In non-development environments, redirect exceptions to a specific error page

// Adds HTTP Strict Transport Security headers (HSTS)
// Instructs browsers to always use HTTPS for future requests
// The default duration is 30 days, configurable for production needs

// Redirects all HTTP requests to HTTPS for secure communication

// Enables serving static files (CSS, JavaScript, images, etc.) from the wwwroot folder
// No server-side processing occurs for these files - they're served directly to the client

// Adds routing capabilities to the middleware pipeline
// Analyzes incoming requests and determines which endpoint should handle them

// Adds authorization middleware to the pipeline
// Ensures that users are authorized to access the requested resources
// Works with [Authorize] attributes on controllers/actions

// Maps controller routes using the conventional routing pattern
// If controller isn't specified, "Home" controller is used
// If action isn't specified, 'Index" action is used
// The id parameter is optional (denoted by ?)

// Starts the web server and begins accepting HTTP requests
// This call is blocking and keeps the application running