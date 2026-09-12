using Entities;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using Repositories;
using RepositoryContracts;
using ServiceContracts;
using Services;


var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders(); 
builder.Logging.AddEventLog();

builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IPersonsService, PersonsService>();
builder.Services.AddScoped<ICountriesService, CountriesService>();

builder.Services.AddScoped<ICountriesRepository, CountriesRepository>();
builder.Services.AddScoped<IPersonsRepository, PersonsRepository>();

if (!builder.Environment.IsEnvironment("Test"))
{
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
    {
         options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    });
    ExcelPackage.License.SetNonCommercialPersonal("Saba-Contacts-Manager");
    Rotativa.AspNetCore.RotativaConfiguration.Setup("wwwroot", wkhtmltopdfRelativePath: "Rotativa");
}

var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.Logger.LogDebug("Debug");
app.Logger.LogInformation("Debug");
app.Logger.LogWarning("Debug");
app.Logger.LogError("Debug");
app.Logger.LogCritical("Debug");

app.UseStaticFiles();
app.MapControllers();

app.Run();

public partial class Program { }