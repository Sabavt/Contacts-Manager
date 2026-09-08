using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;
using Services;

namespace CRUDTests;

public class CountriesServiceTest
{
    private readonly ICountriesService _countriesService;

    public CountriesServiceTest()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
        .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
        .Options;

        ApplicationDbContext dbContext = new ApplicationDbContext(options);

        _countriesService = new CountriesService(dbContext);
    }

    [Fact]
    public async Task AddCountry_CountryNameIsNull()
    {
        //Arrange
        CountryAddRequest? request = new CountryAddRequest() { CountryName = null };

        //Assert
        await Assert.ThrowsAsync<ArgumentException>(async () => await _countriesService.AddCountry(request));
    }

    [Fact]
    public async Task AddCountry_NullCountry()
    {
        //Arrange
        CountryAddRequest? request = null;

        //Assert
        await Assert.ThrowsAsync<ArgumentNullException>(async () => await _countriesService.AddCountry(request));
    }

    [Fact]
    public async Task AddCountry_DublicatedCountryName()
    {
        //Arrange
        CountryAddRequest? request1 = new CountryAddRequest() { CountryName = "USA" };
        CountryAddRequest? request2 = new CountryAddRequest() { CountryName = "USA" };

        //Assert
        await Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            //Act
            await _countriesService.AddCountry(request1);
            await _countriesService.AddCountry(request2);

        });
    }

    [Fact]
    public async Task AddCountry_ProperCountryDetails()
    {
        //Arrange
        CountryAddRequest? request = new CountryAddRequest() { CountryName = "Japan" };

        //Act
        CountryResponse response = await _countriesService.AddCountry(request);
        List<CountryResponse> countries_from_GetAllCountries = await _countriesService.GetAllCountries();

        //Assert
        Assert.True(response.CountryID != Guid.Empty);
        Assert.Contains(response, countries_from_GetAllCountries);
    }

    [Fact]
    //The list of countries should be empty by default (before adding any countries) 
    public async Task GetCountryList_EmptyList()
    {
        //Act
        List<CountryResponse> actualCountry = await _countriesService.GetAllCountries();

        //Assert
        Assert.Empty(actualCountry);
    }

    [Fact]
    public async Task GetCountryList_AddFewCountries()
    {
        //Arrange
        List<CountryAddRequest> country_request_list = [new CountryAddRequest() { CountryName = "USA" }, new CountryAddRequest() { CountryName = "Germany" }, new CountryAddRequest() { CountryName = "Belgium" }];
        List<CountryResponse> countries_list_from_add_country = new List<CountryResponse>();

        //Assert
        foreach (var country_request in country_request_list)
        {
            countries_list_from_add_country.Add(await _countriesService.AddCountry(country_request));
        }

        List<CountryResponse> actualCountryResponseList = await _countriesService.GetAllCountries();

        //Assert
        foreach (var expected_country in countries_list_from_add_country)
        {
            Assert.Contains(expected_country, actualCountryResponseList);
        }
    }

    [Fact]
    public async Task GetCountryByCountryID_NullCountryID()
    {
        //Arrange
        Guid? guid = null;

        //Act
        await _countriesService.GetCountryByCountryID(guid);
    }

    [Fact]
    public async Task GetCountryByCountryID_ValidCountyID()
    {
        //Arrange
        CountryAddRequest? country_add_request = new CountryAddRequest() { CountryName = "Egypt" };
        CountryResponse country_response_from_add = await _countriesService.AddCountry(country_add_request);

        //Act
        CountryResponse? county_response_from_get = await _countriesService.GetCountryByCountryID(country_response_from_add.CountryID);

        //Assert
        Assert.Equal(country_response_from_add, county_response_from_get);
    }
}
