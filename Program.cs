// Entry point of the application - Program.cs in minimal hosting model (introduced in .NET 6+)
// This file configures the application's services, middleware pipeline, and starts the web server

// Creates a new WebApplicationBuilder instance which provides the foundation for configuring the application
// It loads configuration from appsettings.json, environment variables, command line arguments, etc.
var builder = WebApplication.CreateBuilder(args);

// Registers MVC services in the dependency injection container
// This enables controllers, views, model binding, validation, and other MVC features
builder.Services.AddControllersWithViews();

// Builds the WebApplication instance with all registered services
// This finalizes the service configuration and prepares the application for middleware registration
var app = builder.Build();

// Configure the HTTP request pipeline (middleware chain)
// Middleware are executed in the order they are added to the pipeline

// Different middleware configuration based on environment
// Checks environmentVariables inside Properties->launchSettings.json (by default, under https profile)
if (!app.Environment.IsDevelopment())
{
    // In non-development environments, redirect exceptions to a specific error page
    app.UseExceptionHandler("/Home/Error");

    // Adds HTTP Strict Transport Security headers (HSTS)
    // Instructs browsers to always use HTTPS for future requests
    // The default duration is 30 days, configurable for production needs
    app.UseHsts();
}

// Redirects all HTTP requests to HTTPS for secure communication
app.UseHttpsRedirection();

// Enables serving static files (CSS, JavaScript, images, etc.) from the wwwroot folder
// No server-side processing occurs for these files - they're served directly to the client
app.UseStaticFiles();

// Adds routing capabilities to the middleware pipeline
// Analyzes incoming requests and determines which endpoint should handle them
app.UseRouting();

// Adds authorization middleware to the pipeline
// Ensures that users are authorized to access the requested resources
// Works with [Authorize] attributes on controllers/actions
app.UseAuthorization();

// Maps controller routes using the conventional routing pattern
// If controller isn't specified, "Home" controller is used
// If action isn't specified, 'Index" action is used
// The id parameter is optional (denoted by ?)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Starts the web server and begins accepting HTTP requests
// This call is blocking and keeps the application running
app.Run();