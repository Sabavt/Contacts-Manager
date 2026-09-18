using Entities;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using Repositories;
using RepositoryContracts;
using ServiceContracts;
using Services;
using Serilog; 
using ContactsManager;
using ContactsManager.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((HostBuilderContext context, IServiceProvider services, LoggerConfiguration loggerConfiguration) =>
{
    loggerConfiguration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services);
}); 
    
builder.Services.ConfigureServices(builder);  
 
var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandlingMiddleware();
    app.UseExceptionHandler("/Error");
}
app.UseSerilogRequestLogging();
app.UseHttpLogging(); 
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

public partial class Program { }