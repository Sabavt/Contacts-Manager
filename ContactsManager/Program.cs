using Entities;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using ServiceContracts;
using Services;

ExcelPackage.License.SetNonCommercialPersonal("Saba-Contacts-Manager");
Rotativa.AspNetCore.RotativaConfiguration.Setup("wwwroot", wkhtmltopdfRelativePath: "Rotativa");

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();  
builder.Services.AddScoped<IPersonsService, PersonsService>();
builder.Services.AddScoped<ICountriesService, CountriesService>();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build(); 

if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
} 

app.UseStaticFiles();
app.MapControllers();

app.Run();
