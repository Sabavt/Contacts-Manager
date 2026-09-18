using ContactsManager.Core.Domain.IdentityEntities;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
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

        services.AddHttpLogging();

        services.AddControllersWithViews();

        if (!builder.Environment.IsEnvironment
            ("Test"))
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            ExcelPackage.License.SetNonCommercialPersonal("Saba-Contacts-Manager");
            Rotativa.AspNetCore.RotativaConfiguration.Setup("wwwroot", wkhtmltopdfRelativePath: "Rotativa");
        }

        services.AddIdentity<ApplicationUser,
            ApplicationRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders()
            .AddUserStore<UserStore<ApplicationUser, ApplicationRole, ApplicationDbContext, Guid>>()

            .AddRoleStore<RoleStore<ApplicationRole, ApplicationDbContext, Guid>>();

        services.AddAuthorization(opt => opt.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser().Build()
        );
        services.ConfigureApplicationCookie(opt => opt.LoginPath = "/Account/Login");

        return services;
    }
}
