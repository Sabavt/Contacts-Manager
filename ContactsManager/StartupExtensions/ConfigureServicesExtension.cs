using Entities;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using Repositories;
using RepositoryContracts;
using ServiceContracts;
using Services;

namespace ContactsManager;

public static class ConfigureServicesExtension
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services, WebApplicationBuilder builder)
    {
        services.AddHttpLogging();

        services.AddControllersWithViews();

        services.AddScoped<IPersonsGetterService, PersonsGetterServiceWithCompactExcel>();
        services.AddScoped<PersonsGetterService, PersonsGetterService>();
        services.AddScoped<IPersonsDeleterService, PersonsDeleterService>();
        services.AddScoped<IPersonsSorterService, PersonsSorterService>();
        services.AddScoped<IPersonsUpdaterService, PersonsUpdaterService>();
        services.AddScoped<IPersonsAdderService, PersonsAdderService>();

        services.AddScoped<ICountriesAdderService, CountriesAdderService>();
        services.AddScoped<ICountriesGetterService, CountriesGetterService>();
        services.AddScoped<ICountriesUploaderFromExcelService, CountriesUploaderFromExcelService>();

        services.AddScoped<ICountriesRepository, CountriesRepository>();
        services.AddScoped<IPersonsRepository, PersonsRepository>();

        if (!builder.Environment.IsEnvironment("Test"))
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            ExcelPackage.License.SetNonCommercialPersonal("Saba-Contacts-Manager");
            Rotativa.AspNetCore.RotativaConfiguration.Setup("wwwroot", wkhtmltopdfRelativePath: "Rotativa");
        }

        return services;
    }
}
