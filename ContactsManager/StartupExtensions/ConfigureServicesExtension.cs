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

        services.AddScoped<IPersonsSetterService, PersonsGetterService>();
        services.AddScoped<ICountriesService, CountriesService>();

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
